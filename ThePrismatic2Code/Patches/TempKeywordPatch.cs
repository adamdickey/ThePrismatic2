using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using ThePrismatic2.ThePrismatic2Code.Extensions;

namespace ThePrismatic2.ThePrismatic2Code.Patches;

[HarmonyPatch(typeof(AbstractModel), "AfterSideTurnEnd")]
public static class TempKeywordPatch
{
    private static void Postfix(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (side == CombatSide.Player)
        {
            foreach(Creature player in participants)
            {
                IEnumerable<CardModel> enumerable = player.Player?.PlayerCombatState?.AllCards ?? Array.Empty<CardModel>();
                foreach(CardModel card in enumerable)
                {
                    if (card.Keywords.Contains(Keywords.CunningThisTurn))
                    {
                        card.RemoveKeyword(Keywords.CunningThisTurn);
                    }
                    if (card.Keywords.Contains(Keywords.StarboundThisTurn))
                    {
                        card.RemoveKeyword(Keywords.StarboundThisTurn);
                        card.EnergyCost.SetThisCombat(card.EnergyCost.Canonical);
                        card.SetStarCostThisCombat(card.CanonicalStarCost);
                    }
                }
            }
        }
    }
}