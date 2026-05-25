using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace ThePrismatic2.ThePrismatic2Code.Extensions;

[HarmonyPatch(typeof(CardCmd), "Exhaust")]
public static class CunningExhaustPatch
{
    private static async void Postfix(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal = false, bool skipVisuals = false)
    {
        if (!CombatManager.Instance.IsOverOrEnding)
        {
            if (card.Keywords.Contains(Keywords.Cunning))
            {
                await CardCmd.AutoPlay(choiceContext, card, null, AutoPlayType.SlyDiscard);
            }
        }
    }
}