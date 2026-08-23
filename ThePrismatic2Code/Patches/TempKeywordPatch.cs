using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using ThePrismatic2.ThePrismatic2Code.Extensions;

namespace ThePrismatic2.ThePrismatic2Code.Patches;

public class RemoveTempKeywordSingleton() : CustomSingletonModel(true, false)
{
    public override Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (side != CombatSide.Player) return Task.CompletedTask;
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
                    if (card.EnergyCost.GetWithModifiers(CostModifiers.All) + card.GetStarCostWithModifiers() == card.EnergyCost.Canonical + card.CanonicalStarCost)
                    {
                        card.EnergyCost.SetThisCombat(card.EnergyCost.Canonical);
                        card.SetStarCostThisCombat(card.CanonicalStarCost);
                    }
                    else if (card.CanonicalStarCost == -1)
                    {
                        card.EnergyCost.SetThisCombat(card.EnergyCost.GetWithModifiers(CostModifiers.All) + card.GetStarCostWithModifiers());
                        card.SetStarCostThisCombat(card.CanonicalStarCost);
                    }
                }
            }
        }
        return Task.CompletedTask;
    }
}