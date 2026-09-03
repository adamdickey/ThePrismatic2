using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Models.Relics;
using ThePrismatic2.ThePrismatic2Code.Relics;
using PrismaticChar = ThePrismatic2.ThePrismatic2Code.Character.ThePrismatic2;

namespace ThePrismatic2.ThePrismatic2Code.Patches;

/// <summary>
/// Adds extra relics to the option pools of the <i>base game</i> ancients.
///
/// Each basegame ancient keeps its candidates in one or more private
/// <c>IEnumerable&lt;EventOption&gt;</c> properties, and its <c>GenerateInitialOptions</c>
/// picks the three offered relics out of those. Patching a pool property therefore
/// covers both the offered relics and the compendium's "all possible options" listing,
/// because <c>AllPossibleOptions</c> is itself built from the same pools.
///
/// Register relics in <see cref="RegisterRelics"/>; see <see cref="Pools"/> for the
/// property name each ancient uses. Darv has no pools and is handled by
/// <see cref="DarvRelicInjection"/>, which this class installs alongside its own patches.
/// </summary>
/// 
/// Todo ensure that all relics are in the right order, ie no archaic tooth in second slot of orobas
public static class AncientRelicInjection
{

    /// <summary>Add Ancient relics here :)</summary>
    private static void RegisterAncientRelics()
    {
        // EXAMPLE ENTRIES - edit freely. Character type gates the relic to that character;
        // use the two-argument overload to offer it to everyone.
        //Add<Tanx, PrismaticChar, SadisticDagger>(Pools.Tanx);
        //Add<Vakuu, PrismaticChar, RealityBox>(Pools.VakuuPool2);
        Add<Orobas, PrismaticChar, DivineDestiny>(Pools.OptionPool1);
        //Add<Orobas, PrismaticChar, Bookmark>(Pools.OptionPool1);

        // Darv is special, you have to do his differently:
        DarvRelicInjection.Add<PrismaticChar, Brimstone>();   
        //DarvRelicInjection.Add<PrismaticChar, RealityBox>();   
    }



    /// <summary>The pool property each basegame ancient actually draws its offered relics from.</summary>
    public static class Pools
    {
        public const string Tanx = "BaseOptionPool";
        public const string Nonupeipe = "OptionPool";

        // Orobas, Pael and Tezcatara each have three, one per offered option.
        public const string OptionPool1 = "OptionPool1";
        public const string OptionPool2 = "OptionPool2";
        public const string OptionPool3 = "OptionPool3";

        // Vakuu names its three pools differently.
        public const string VakuuPool1 = "Pool1";
        public const string VakuuPool2 = "Pool2";
        public const string VakuuPool3 = "Pool3";

        // Neow. Relics here are additionally filtered by RelicModel.IsAllowedAtNeow.
        public const string NeowPositive = "PositiveOptions";
        public const string NeowCurses = "CurseOptions";
    }

    /// <summary>Offer <typeparamref name="TRelic"/> at <typeparamref name="TAncient"/>, only as <typeparamref name="TCharacter"/>.</summary>
    public static void Add<TAncient, TCharacter, TRelic>(string poolProperty)
        where TAncient : AncientEventModel
        where TCharacter : CharacterModel
        where TRelic : RelicModel
        => Entries.Add(new Entry(typeof(TAncient), poolProperty, () => ModelDb.Relic<TRelic>(),
            typeof(TRelic).Name, typeof(TCharacter)));

    /// <summary>Offer <typeparamref name="TRelic"/> at <typeparamref name="TAncient"/> for every character.</summary>
    public static void Add<TAncient, TRelic>(string poolProperty)
        where TAncient : AncientEventModel
        where TRelic : RelicModel
        => Entries.Add(new Entry(typeof(TAncient), poolProperty, () => ModelDb.Relic<TRelic>(),
            typeof(TRelic).Name, null));

    private sealed record Entry(Type Ancient, string PoolProperty, Func<RelicModel> Relic, string RelicName, Type? Character);
    private static readonly List<Entry> Entries = [];
    private static readonly Dictionary<MethodBase, List<Entry>> ByGetter = new();

    // protected EventOption AncientEventModel.RelicOption(RelicModel relic, string pageName, string customDonePage)
    private static readonly MethodInfo? RelicOptionMethod = AccessTools.Method(
        typeof(AncientEventModel), "RelicOption",
        [typeof(RelicModel), typeof(string), typeof(string)]);

    /// <summary>Call once from the mod initializer, after <c>PatchAll</c>.</summary>
    public static void Install(Harmony harmony)
    {
        if (RelicOptionMethod is null)
        {
            MainFile.Logger.Error(
                "AncientEventModel.RelicOption(RelicModel, string, string) not found; no ancient relics added.");
            return;
        }

        RegisterAncientRelics();

        HarmonyMethod postfix = new(AccessTools.Method(typeof(AncientRelicInjection), nameof(PoolPostfix)));

        foreach (IGrouping<(Type Ancient, string Pool), Entry> group in
                 Entries.GroupBy(entry => (entry.Ancient, Pool: entry.PoolProperty)))
        {
            MethodInfo? getter = AccessTools.Property(group.Key.Ancient, group.Key.Pool)?.GetGetMethod(true);
            if (getter is null)
            {
                MainFile.Logger.Error(
                    $"{group.Key.Ancient.Name} has no pool property '{group.Key.Pool}'. " +
                    $"Pools on that ancient: {string.Join(", ", PoolNames(group.Key.Ancient))}");
                continue;
            }

            ByGetter[getter] = group.ToList();
            harmony.Patch(getter, postfix: postfix);
        }

        DarvRelicInjection.Install(harmony);
    }

    public static void PoolPostfix(MethodBase __originalMethod, AncientEventModel __instance, ref IEnumerable<EventOption> __result)
    {
        if (!ByGetter.TryGetValue(__originalMethod, out List<Entry>? entries)) return;

        List<EventOption> options = __result?.ToList() ?? [];

        foreach (Entry entry in entries)
        {
            if (!MatchesCharacter(__instance, entry.Character)) continue;

            try
            {
                RelicModel relic = entry.Relic().ToMutable();
                if (RelicOptionMethod!.Invoke(__instance, [relic, "INITIAL", null]) is EventOption option)
                {
                    options.Add(option);
                }
            }
            catch (Exception e)
            {
                MainFile.Logger.Error(
                    $"Could not add {entry.RelicName} to {entry.Ancient.Name}.{entry.PoolProperty}: {e}");
            }
        }

        __result = options;
    }

    private static bool MatchesCharacter(AncientEventModel ancient, Type? character)
    {
        if (character is null) return true;

        // The immutable prototype has no Owner; it backs the compendium's list of
        // everything an ancient can offer, so don't filter it by character there.
        if (ancient is not { IsMutable: true, Owner: not null }) return true;

        return character.IsInstanceOfType(ancient.Owner.Character);
    }

    private static IEnumerable<string> PoolNames(Type ancient) =>
        ancient.GetProperties(BindingFlags.Instance | BindingFlags.Public |
                              BindingFlags.NonPublic | BindingFlags.DeclaredOnly)
            .Where(property => typeof(IEnumerable<EventOption>).IsAssignableFrom(property.PropertyType))
            .Select(property => property.Name);
}
