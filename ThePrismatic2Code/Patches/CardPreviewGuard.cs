using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace ThePrismatic2.ThePrismatic2Code.Patches;

/// <summary>
/// Keeps a broken card preview from freezing a run.
///
/// Card visuals are built inside the await chain that adds a card to a pile:
///   CardPileCmd.Add -> CreateCardNodeAndUpdateVisuals -> NCard.UpdateVisuals
///     -> CardModel.UpdateDynamicVarPreview -> CalculatedVar.Calculate
///
/// Anything thrown down there kills the Add, so the card is half-created, the draw never finishes
/// and the turn cannot advance - a softlock with a card fragment stuck in the corner of the screen.
/// A cosmetically wrong number on one card is enormously preferable to that, so these two patches
/// turn any such throw into a logged warning.
///
/// <see cref="MissingMultiplierGuard"/> handles the specific case seen in the wild and names the
/// culprit; <see cref="PreviewFinalizer"/> is the catch-all behind it.
/// </summary>
public static class CardPreviewGuard
{
    /// <summary>Card/var pairs already reported, so a per-frame redraw can't flood the log.</summary>
    private static readonly HashSet<string> Reported = [];

    internal static bool ShouldReport(string what) => Reported.Add(what);

    internal static string Describe(object? model) =>
        model is AbstractModel abstractModel
            ? $"{abstractModel.GetType().Name} ({abstractModel.Id.Entry})"
            : model?.GetType().Name ?? "<unknown>";
}

/// <summary>
/// A <see cref="CalculatedVar"/> with no multiplier delegate throws
/// "Extra multiplier calc must be specified!" the moment anything tries to render it.
///
/// Every calculated var in this mod does set one at declaration, so a null here means the copy of
/// the card being previewed lost it - which is worth knowing about. Log which card and which var,
/// then return 0 so the preview finishes instead of taking the run down with it.
/// </summary>
[HarmonyPatch(typeof(CalculatedVar), nameof(CalculatedVar.Calculate))]
public static class MissingMultiplierGuard
{
    private static readonly FieldInfo? MultiplierCalc = AccessTools.Field(typeof(CalculatedVar), "_multiplierCalc");
    private static readonly FieldInfo? VarOwner = AccessTools.Field(typeof(DynamicVar), "_owner");

    public static bool Prefix(CalculatedVar __instance, ref decimal __result)
    {
        // Can't find the field (game update?) - let the original run and behave as before.
        if (MultiplierCalc is null) return true;
        if (MultiplierCalc.GetValue(__instance) is not null) return true;

        string what = $"{CardPreviewGuard.Describe(VarOwner?.GetValue(__instance))} -> var '{__instance.Name}'";

        if (CardPreviewGuard.ShouldReport($"multiplier:{what}"))
        {
            MainFile.Logger.Error(
                $"CALCULATED VAR HAS NO MULTIPLIER: {what}. " +
                "Showing 0 for it rather than softlocking the fight.");
        }

        __result = 0m;
        return false;
    }
}

/// <summary>
/// Catch-all for anything else that throws while building a card's preview values. Returning null
/// from a finalizer swallows the exception, so <c>CardPileCmd.Add</c> completes and the draw
/// carries on; the card just shows whatever values it had already resolved.
/// </summary>
[HarmonyPatch(typeof(CardModel), "UpdateDynamicVarPreview")]
public static class PreviewFinalizer
{
    public static Exception? Finalizer(Exception? __exception, CardModel __instance)
    {
        if (__exception is null) return null;

        string what = CardPreviewGuard.Describe(__instance);

        if (CardPreviewGuard.ShouldReport($"preview:{what}:{__exception.GetType().Name}"))
        {
            MainFile.Logger.Error($"CARD PREVIEW FAILED for {what}, swallowing it: {__exception}");
        }

        return null;
    }
}
