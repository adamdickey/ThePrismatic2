using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using PrismaticChar = ThePrismatic2.ThePrismatic2Code.Character.ThePrismatic2;

namespace ThePrismatic2.ThePrismatic2Code.Patches;

/// Stops named relics from ever being offered to The Prismatic.
public static class BlockedRelics
{
    /// <summary>
    /// ===== EDIT THIS LIST =====
    ///
    /// One line per relic The Prismatic should never be offered. Copy an existing line and change
    /// the name inside the angle brackets - see BaseGameRelicNames.txt for every name available.
    /// </summary>
    private static IEnumerable<RelicModel> BlockedForPrismatic() =>
    [
        // Deleted relics these:
        ModelDb.Relic<SeaGlass>(),
        ModelDb.Relic<PrismaticGem>(),
        ModelDb.Relic<Sozu>(),
        ModelDb.Relic<Ectoplasm>(),
        ModelDb.Relic<Kaleidoscope>(),
        ModelDb.Relic<Cauldron>(),
    ];

    /// <summary>True when this relic is on the list above and this player is The Prismatic.</summary>
    public static bool IsBlockedFor(Player? player, RelicModel? relic)
        => player?.Character is PrismaticChar && IsBlocked(relic);

    private static List<ModelId>? _blockedIds;

    private static bool IsBlocked(RelicModel? relic)
    {
        if (relic is null) return false;

        foreach (ModelId id in BlockedIds())
        {
            if (id == relic.Id) return true;
        }

        return false;
    }

    private static List<ModelId> BlockedIds()
    {
        if (_blockedIds is not null) return _blockedIds;

        List<ModelId> ids = [];
        bool complete = true;

        foreach (RelicModel relic in BlockedForPrismatic())
        {
            // Only happens if the model database isn't loaded yet; don't cache a short list.
            if (relic is null)
            {
                complete = false;
                continue;
            }

            ids.Add(relic.Id);
        }

        if (complete) _blockedIds = ids;
        return ids;
    }

    // -------------------------------------------------------------------------------------
    //  Road 1 - the grab bag (combat rewards, chests, shops, most events)
    // -------------------------------------------------------------------------------------

    /// <summary>
    /// Two lists have to be cleaned, not one: the per-rarity piles the game draws from, and the
    /// master list it keeps to refill a pile once that rarity runs dry. Clearing only the piles
    /// would let a blocked relic reappear late in a long run.
    /// </summary>
    [HarmonyPatch(typeof(RelicGrabBag), nameof(RelicGrabBag.Populate), typeof(Player), typeof(Rng))]
    public static class GrabBagPatch
    {
        private static readonly FieldInfo? MasterList =
            AccessTools.Field(typeof(RelicGrabBag), "_originalRelics");

        public static void Postfix(RelicGrabBag __instance, Player player)
        {
            if (player?.Character is not PrismaticChar) return;

            try
            {
                int blocked = 0;

                foreach (RelicModel relic in BlockedForPrismatic())
                {
                    if (relic is null) continue;

                    __instance.Remove(relic);

                    if (MasterList?.GetValue(__instance) is List<RelicModel> master)
                    {
                        master.RemoveAll(candidate => candidate.Id == relic.Id);
                    }

                    blocked++;
                }

                if (blocked > 0)
                {
                    MainFile.Logger.Info($"Blocked {blocked} relic(s) from The Prismatic's grab bag.");
                }
            }
            catch (Exception e)
            {
                MainFile.Logger.Error($"Could not block relics in the grab bag: {e}");
            }
        }
    }

    // -------------------------------------------------------------------------------------
    //  Road 2 - ancients and Neow
    // -------------------------------------------------------------------------------------

    /// <summary>
    /// Call once from the mod initializer. Finds every option pool on every ancient in the game
    /// (rather than hard coding their names, so this keeps working if the game adds more) and
    /// filters blocked relics out of each one.
    /// </summary>
    public static void InstallAncientBlocking(Harmony harmony)
    {
        // Pools come back as either IEnumerable<EventOption> or List<EventOption> depending on the
        // ancient, and Harmony needs a postfix whose __result type matches exactly, so there is one
        // of each.
        HarmonyMethod enumerablePostfix = new(AccessTools.Method(typeof(BlockedRelics), nameof(PoolPostfix)));
        HarmonyMethod listPostfix = new(AccessTools.Method(typeof(BlockedRelics), nameof(PoolPostfixList)));
        int pools = 0;

        foreach (Type ancient in AncientTypes())
        {
            foreach (PropertyInfo property in ancient.GetProperties(
                         BindingFlags.Instance | BindingFlags.Public |
                         BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
            {
                // Only the pools. A single-relic property can't be filtered without leaving a hole
                // where an option should be, so the safety net below handles those instead.
                HarmonyMethod? postfix =
                    property.PropertyType == typeof(IEnumerable<EventOption>) ? enumerablePostfix
                    : property.PropertyType == typeof(List<EventOption>) ? listPostfix
                    : null;

                if (postfix is null) continue;

                MethodInfo? getter = property.GetGetMethod(true);
                if (getter is null || getter.IsAbstract) continue;

                try
                {
                    harmony.Patch(getter, postfix: postfix);
                    pools++;
                }
                catch (Exception e)
                {
                    MainFile.Logger.Error($"Could not filter {ancient.Name}.{property.Name}: {e}");
                }
            }
        }

        // Last line of defense: catches relics an ancient names directly rather than drawing from
        // a pool (Tanx's Tri-Boomerang, Pael's own relics, anything Darv rolled, and so on).
        MethodBase? wrapper = AccessTools.Method(typeof(AncientEventModel), "GenerateInitialOptionsWrapper");

        if (wrapper is null)
        {
            MainFile.Logger.Error("AncientEventModel.GenerateInitialOptionsWrapper not found; " +
                                  "ancients can still offer blocked relics they name directly.");
        }
        else
        {
            harmony.Patch(wrapper, postfix: new HarmonyMethod(
                AccessTools.Method(typeof(BlockedRelics), nameof(SafetyNetPostfix))));
        }

        MainFile.Logger.Info($"Watching {pools} ancient option pool(s) for blocked relics.");
    }

    private static IEnumerable<Type> AncientTypes()
        => typeof(AncientEventModel).Assembly.GetTypes()
            .Where(type => !type.IsAbstract && typeof(AncientEventModel).IsAssignableFrom(type));

    /// <summary>
    /// Removes blocked relics from an ancient's pool before it picks. Because this happens at the
    /// pool stage the ancient simply picks something else, so you still get three options.
    /// </summary>
    public static void PoolPostfix(AncientEventModel __instance, ref IEnumerable<EventOption> __result)
    {
        if (__result is null) return;

        // No owner means this is the prototype backing the compendium's "everything this ancient
        // can offer" list, which should stay complete.
        if (__instance.Owner is null || __instance.Owner.Character is not PrismaticChar) return;

        try
        {
            __result = __result.Where(option => !IsBlocked(option?.Relic)).ToList();
        }
        catch (Exception e)
        {
            MainFile.Logger.Error($"Could not filter an option pool on {__instance.GetType().Name}: {e}");
        }
    }

    /// <summary>Same as <see cref="PoolPostfix"/>, for the pools typed as List rather than IEnumerable.</summary>
    public static void PoolPostfixList(AncientEventModel __instance, ref List<EventOption> __result)
    {
        if (__result is null) return;
        if (__instance.Owner is null || __instance.Owner.Character is not PrismaticChar) return;

        try
        {
            __result = __result.Where(option => !IsBlocked(option?.Relic)).ToList();
        }
        catch (Exception e)
        {
            MainFile.Logger.Error($"Could not filter an option pool on {__instance.GetType().Name}: {e}");
        }
    }

    /// <summary>
    /// Ancients whose offered slots each come from a different pool, listed slot by slot. A
    /// replacement for a blocked relic is drawn from its slot's pool, so the order is always
    /// Pool 1 relic -> Pool 2 relic -> Pool 3 relic. Ancients not listed here (Tanx, Nonupeipe,
    /// Darv) draw every slot from one shared pool, so any of their relics fits any slot.
    /// </summary>
    private static readonly Dictionary<Type, string[]> SlotPools = new()
    {
        [typeof(Orobas)] = [AncientRelicInjection.Pools.OptionPool1, AncientRelicInjection.Pools.OptionPool2, AncientRelicInjection.Pools.OptionPool3],
        [typeof(Pael)] = [AncientRelicInjection.Pools.OptionPool1, AncientRelicInjection.Pools.OptionPool2, AncientRelicInjection.Pools.OptionPool3],
        [typeof(Tezcatara)] = [AncientRelicInjection.Pools.OptionPool1, AncientRelicInjection.Pools.OptionPool2, AncientRelicInjection.Pools.OptionPool3],
        [typeof(Vakuu)] = [AncientRelicInjection.Pools.VakuuPool1, AncientRelicInjection.Pools.VakuuPool2, AncientRelicInjection.Pools.VakuuPool3],
        // Neow offers two positives, then the curse last.
        [typeof(Neow)] = [AncientRelicInjection.Pools.NeowPositive, AncientRelicInjection.Pools.NeowPositive, AncientRelicInjection.Pools.NeowCurses],
    };

    /// <summary>
    /// Runs after an ancient has settled on the options it wants to show, and replaces any blocked
    /// relic still standing. This is what catches relics an ancient names directly instead of
    /// drawing from a pool, where there was no pool to filter (Orobas's Sea Glass and Prismatic
    /// Gem, for example).
    ///
    /// Each blocked option is swapped out in place, from the pool that slot was drawn from, so the
    /// slot order is kept - see <see cref="PickReplacement"/>. Only when nothing at all is left to
    /// replace it with is the option dropped.
    /// </summary>
    public static void SafetyNetPostfix(AncientEventModel __instance, ref IReadOnlyList<EventOption> __result)
    {
        if (__result is null) return;
        if (__instance.Owner is null || __instance.Owner.Character is not PrismaticChar) return;

        try
        {
            List<EventOption?> options = [..__result];
            int replaced = 0;
            int dropped = 0;

            for (int slot = 0; slot < options.Count; slot++)
            {
                EventOption? blocked = options[slot];
                if (!IsBlocked(blocked?.Relic)) continue;

                EventOption? replacement = PickReplacement(__instance, slot, blocked!.Relic!.Rarity, options);
                options[slot] = replacement;

                if (replacement is null) dropped++;
                else replaced++;
            }

            if (replaced == 0 && dropped == 0) return;

            MainFile.Logger.Info(
                $"{__instance.GetType().Name}: replaced {replaced} and dropped {dropped} blocked relic option(s).");

            __result = options.OfType<EventOption>().ToList();
        }
        catch (Exception e)
        {
            MainFile.Logger.Error($"Could not filter {__instance.GetType().Name}'s final options: {e}");
        }
    }

    /// <summary>
    /// Picks a stand-in for the blocked option in <paramref name="slot"/>, preferring the rarity of
    /// what it replaces and never duplicating an option already on offer.
    ///
    /// The candidates are the slot's own pool when the ancient is in <see cref="SlotPools"/>, and
    /// AllPossibleOptions otherwise (or if that pool has nothing left). Both have already had blocked
    /// relics filtered out by <see cref="PoolPostfix"/> and our added relics put in by
    /// <see cref="AncientRelicInjection"/>, since reading them goes through the patched getters.
    /// </summary>
    private static EventOption? PickReplacement(AncientEventModel ancient, int slot, RelicRarity rarity,
                                                List<EventOption?> offered)
    {
        List<EventOption> candidates = Usable(ancient, SlotPool(ancient, slot), offered);

        if (candidates.Count == 0)
        {
            if (SlotPools.ContainsKey(ancient.GetType()))
            {
                MainFile.Logger.Warn(
                    $"{ancient.GetType().Name}: slot {slot + 1}'s pool has nothing left to offer, " +
                    "replacing from the ancient's other relics instead.");
            }

            candidates = Usable(ancient, AllPossible(ancient), offered);
        }

        if (candidates.Count == 0) return null;

        List<EventOption> sameRarity = candidates.Where(o => o.Relic!.Rarity == rarity).ToList();
        return ancient.Rng.NextItem(sameRarity.Count > 0 ? sameRarity : candidates);
    }

    private static List<EventOption> Usable(AncientEventModel ancient, IEnumerable<EventOption> options,
                                            List<EventOption?> offered)
        => options
            .Where(option => option?.Relic is not null && !IsBlocked(option.Relic))
            .Where(option => ancient is not Neow || option.Relic!.IsAllowedAtNeow(ancient.Owner!))
            .Where(option => !offered.Any(o => o?.Relic is not null && o.Relic.Id == option.Relic!.Id))
            .ToList();

    private static IEnumerable<EventOption> SlotPool(AncientEventModel ancient, int slot)
    {
        if (!SlotPools.TryGetValue(ancient.GetType(), out string[]? pools) || slot >= pools.Length) return [];

        try
        {
            return AccessTools.Property(ancient.GetType(), pools[slot])?.GetValue(ancient)
                as IEnumerable<EventOption> ?? [];
        }
        catch (Exception e)
        {
            MainFile.Logger.Error($"Could not read {ancient.GetType().Name}.{pools[slot]}: {e}");
            return [];
        }
    }

    private static IEnumerable<EventOption> AllPossible(AncientEventModel ancient)
    {
        try
        {
            return (ancient.AllPossibleOptions ?? []).ToList();
        }
        catch (Exception e)
        {
            MainFile.Logger.Error($"Could not read {ancient.GetType().Name}.AllPossibleOptions: {e}");
            return [];
        }
    }
}
