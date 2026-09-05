using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;

namespace ThePrismatic2.ThePrismatic2Code.Patches;

/// <summary>
/// DIAGNOSTIC - safe to delete once the Osty question is settled.
///
/// A creature only dies if every hook listener agrees. Hook.ShouldDie walks the listeners and the
/// first one that returns false vetoes the death, leaving the creature standing on 0 HP; the game
/// then calls Hook.AfterPreventingDeath with whoever objected.
///
/// AbstractModel.ShouldDie returns true by default and nothing in this mod overrides it, so if
/// Osty is surviving at 0 HP something else is casting the veto. This logs exactly who.
/// </summary>
[HarmonyPatch(typeof(Hook), nameof(Hook.AfterPreventingDeath))]
public static class DeathPreventionLogger
{
    public static void Prefix(AbstractModel preventer, Creature creature)
    {
        MainFile.Logger.Error(
            $"DEATH PREVENTED: {creature?.Name ?? "?"} on {creature?.CurrentHp ?? -1} HP " +
            $"was kept alive by {preventer?.GetType().Name ?? "<null>"} ({preventer?.Id.Entry ?? "-"}).");
    }
}
