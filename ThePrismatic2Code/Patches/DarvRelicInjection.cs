using System.Collections;
using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;

namespace ThePrismatic2.ThePrismatic2Code.Patches;

/// <summary>
/// Darv is the one basegame ancient without option pool properties, so neither
/// <see cref="AncientRelicInjection"/> nor <see cref="BlockedRelics"/> can reach it the usual way.
///
/// Instead it keeps a <c>private static readonly List&lt;ValidRelicSet&gt; _validRelicSets</c>, where
/// <c>ValidRelicSet</c> is a private nested struct of
/// <c>(Func&lt;Player, bool&gt; filter, RelicModel[] relics)</c>. <c>GenerateInitialOptions</c> keeps the
/// sets whose filter accepts the owner, turns each surviving set into <i>one</i> option (a random relic
/// out of that set), shuffles, then either takes 3 or takes 2 and appends Dusty Tome. So one added set
/// == one more candidate competing for the three slots.
///
/// This class does two jobs against that list:
///
/// * <b>Adding.</b> The field is <c>initonly</c>, but that only stops it being reassigned - the List it
///   points at is still mutable, so we just add to it. It has to happen lazily: Darv's static
///   constructor builds the list out of <c>ModelDb</c>, so touching the field before the model database
///   is populated throws a TypeInitializationException, which would poison Darv permanently.
///
/// * <b>Blocking.</b> Blocked relics are per character, but the set list is shared by everyone, so we
///   can't just delete them. Instead we swap in a filtered copy of the list for the duration of one
///   <c>GenerateInitialOptions</c> call and put the original back afterwards. Filtering at that stage
///   (rather than dropping the finished option) means Darv picks a replacement and you still get three
///   choices.
/// </summary>
public static class DarvRelicInjection
{
    private sealed record DarvSet(Func<Player, bool> Filter, List<Func<RelicModel>> Relics, string Description);

    private const int MaxAttempts = 5;

    private static readonly List<DarvSet> Sets = [];

    private static FieldInfo? _field;
    private static ConstructorInfo? _setConstructor;
    private static FieldInfo? _filterField;
    private static FieldInfo? _relicsField;
    private static bool _registered;
    private static int _attempts;

    /// <summary>Set list as it was before we filtered it, or null when nothing is swapped out.</summary>
    private static List<object>? _savedSets;

    /// <summary>Offer <typeparamref name="TRelic"/> at Darv, only while playing <typeparamref name="TCharacter"/>.</summary>
    public static void Add<TCharacter, TRelic>()
        where TCharacter : CharacterModel
        where TRelic : RelicModel
        => AddSet<TCharacter>(typeof(TRelic).Name, () => ModelDb.Relic<TRelic>());

    /// <summary>
    /// Add one candidate that picks at random between several relics, which is how Darv's own
    /// entries are built. Relics are resolved lazily, so this is safe to call at mod init.
    /// </summary>
    public static void AddSet<TCharacter>(string description, params Func<RelicModel>[] relics)
        where TCharacter : CharacterModel
        => Sets.Add(new DarvSet(player => player?.Character is TCharacter, [..relics], description));

    public static void Install(Harmony harmony)
    {
        _field = AccessTools.Field(typeof(Darv), "_validRelicSets");
        Type? setType = _field?.FieldType.GetGenericArguments().FirstOrDefault();
        _setConstructor = setType is null
            ? null
            : AccessTools.Constructor(setType, [typeof(Func<Player, bool>), typeof(RelicModel[])]);
        _filterField = setType is null ? null : AccessTools.Field(setType, "filter");
        _relicsField = setType is null ? null : AccessTools.Field(setType, "relics");

        if (_field is null || _setConstructor is null || _filterField is null || _relicsField is null)
        {
            MainFile.Logger.Error(
                "Darv._validRelicSets or ValidRelicSet(Func<Player, bool>, RelicModel[]) not found; " +
                "relics can't be added to or blocked at Darv.");
            return;
        }

        MethodBase? generate = AccessTools.Method(typeof(Darv), "GenerateInitialOptions");
        MethodBase? allPossible = AccessTools.PropertyGetter(typeof(Darv), "AllPossibleOptions");

        if (generate is null || allPossible is null)
        {
            MainFile.Logger.Error("Could not find Darv's _validRelicSets readers to patch.");
            return;
        }

        harmony.Patch(generate,
            prefix: new HarmonyMethod(AccessTools.Method(typeof(DarvRelicInjection), nameof(BeforeGenerate))),
            postfix: new HarmonyMethod(AccessTools.Method(typeof(DarvRelicInjection), nameof(AfterGenerate))));

        // The compendium's "everything Darv can offer" list should stay complete, so this one only
        // needs our added sets to exist - no blocking.
        harmony.Patch(allPossible,
            prefix: new HarmonyMethod(AccessTools.Method(typeof(DarvRelicInjection), nameof(EnsureRegistered))));
    }

    public static void BeforeGenerate(Darv __instance)
    {
        // If a previous call threw before its postfix ran, put the list back first.
        RestoreSets();

        EnsureRegistered();
        FilterBlockedForOwner(__instance);
    }

    public static void AfterGenerate() => RestoreSets();

    /// <summary>
    /// Adds our sets exactly once, and only when every relic resolves, so a half-populated
    /// ModelDb can't leave a partial or duplicated entry.
    /// </summary>
    public static void EnsureRegistered()
    {
        if (_registered || Sets.Count == 0 || _field is null || _setConstructor is null) return;
        if (_attempts++ >= MaxAttempts) return;

        try
        {
            List<object> boxed = [];

            foreach (DarvSet set in Sets)
            {
                RelicModel[] relics = set.Relics.Select(get => get()).OfType<RelicModel>().ToArray();

                if (relics.Length != set.Relics.Count)
                {
                    MainFile.Logger.Warn(
                        $"Darv: could not resolve every relic for '{set.Description}' yet, will retry.");
                    return;
                }

                boxed.Add(_setConstructor.Invoke([set.Filter, relics]));
            }

            if (_field.GetValue(null) is not IList sets)
            {
                MainFile.Logger.Error("Darv._validRelicSets was not a list; no relics added to Darv.");
                _registered = true;
                return;
            }

            foreach (object set in boxed) sets.Add(set);

            _registered = true;
            MainFile.Logger.Info($"Darv: added {boxed.Count} relic set(s); {sets.Count} total.");
        }
        catch (Exception e)
        {
            MainFile.Logger.Error($"Darv: could not add relic sets: {e}");
        }
    }

    /// <summary>
    /// Temporarily rewrites the shared set list with the owner's blocked relics taken out. A set
    /// that loses every relic disappears entirely. Does nothing when nothing is blocked.
    /// </summary>
    private static void FilterBlockedForOwner(Darv darv)
    {
        if (_field is null || _setConstructor is null || _filterField is null || _relicsField is null) return;

        Player? owner = darv.Owner;
        if (owner is null) return;

        try
        {
            if (_field.GetValue(null) is not IList sets) return;

            List<object> original = [];
            foreach (object? set in sets)
            {
                if (set is not null) original.Add(set);
            }

            List<object> filtered = [];
            bool changed = false;

            foreach (object set in original)
            {
                if (_relicsField.GetValue(set) is not RelicModel[] relics)
                {
                    filtered.Add(set);
                    continue;
                }

                RelicModel[] kept = relics
                    .Where(relic => !BlockedRelics.IsBlockedFor(owner, relic))
                    .ToArray();

                if (kept.Length == relics.Length)
                {
                    filtered.Add(set);
                    continue;
                }

                changed = true;

                // Every relic in this set was blocked, so the set contributes nothing.
                if (kept.Length == 0) continue;

                if (_filterField.GetValue(set) is Func<Player, bool> filter)
                {
                    filtered.Add(_setConstructor.Invoke([filter, kept]));
                }
            }

            if (!changed) return;

            _savedSets = original;
            sets.Clear();
            foreach (object set in filtered) sets.Add(set);
        }
        catch (Exception e)
        {
            MainFile.Logger.Error($"Darv: could not filter blocked relics: {e}");
            RestoreSets();
        }
    }

    private static void RestoreSets()
    {
        if (_savedSets is null) return;

        try
        {
            if (_field?.GetValue(null) is IList sets)
            {
                sets.Clear();
                foreach (object set in _savedSets) sets.Add(set);
            }
        }
        catch (Exception e)
        {
            MainFile.Logger.Error($"Darv: could not restore the relic set list: {e}");
        }
        finally
        {
            _savedSets = null;
        }
    }
}
