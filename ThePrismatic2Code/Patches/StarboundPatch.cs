using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using ThePrismatic2.ThePrismatic2Code.Extensions;

namespace ThePrismatic2.ThePrismatic2Code.Patches;

public class StarboundSingleton() : CustomSingletonModel(HookType.Combat)
{
    public override Task AfterEnergyReset(Player player)
    {
        IEnumerable<CardModel> enumerable = player.PlayerCombatState?.AllCards ?? Array.Empty<CardModel>();
        foreach (CardModel card in enumerable)
        {
            UpdateStarbound(card);
        }
        return Task.CompletedTask;
    }
    
    public override Task AfterCardEnteredCombat(CardModel card)
    {
        UpdateStarbound(card);
        return Task.CompletedTask;
    }

    public override Task AfterStarsGained(int amount, Player gainer)
    {
        IEnumerable<CardModel> enumerable = gainer.PlayerCombatState?.AllCards.Where(c => c.Pile != PileType.Play.GetPile(gainer)) ?? Array.Empty<CardModel>();
        foreach (CardModel card in enumerable)
        {
            UpdateStarbound(card);
        }
        return Task.CompletedTask;
    }

    public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.ResultPile == PileType.Hand)
        {
            UpdateStarbound(cardPlay.Card, true);
        }
        IEnumerable<CardModel> enumerable = cardPlay.Card.Owner.PlayerCombatState?.AllCards.Where(c => c.Pile != PileType.Play.GetPile(cardPlay.Card.Owner)) ?? Array.Empty<CardModel>();
        foreach (CardModel card in enumerable)
        {
            UpdateStarbound(card);
        }
        return Task.CompletedTask;
    }
    
    public override Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        IEnumerable<CardModel> enumerable = card.Owner.PlayerCombatState?.AllCards.Where(c => c.Pile != PileType.Play.GetPile(card.Owner)) ?? Array.Empty<CardModel>();
        foreach (CardModel card2 in enumerable)
        {
            UpdateStarbound(card2);
        }
        return Task.CompletedTask;
    }

    public static void Refresh(CardModel card) => UpdateStarbound(card);

    // EnergyCost.Canonical is the printed cost and ignores upgrades (e.g. a -1 energy upgrade), so use the unmodified base.
    private static int BaseEnergyCost(CardModel card) => card.EnergyCost.GetWithModifiers(CostModifiers.None);

    private static void UpdateStarbound(CardModel card, bool permanentCostChange = false)
    {
        if (card.CombatState == null || (!card.Keywords.Contains(Keywords.Starbound) && !card.Keywords.Contains(Keywords.StarboundThisTurn))) return;
        
        int cardEnergy = Math.Max(0, card.EnergyCost.GetWithModifiers(CostModifiers.Local));
        int cardStars = Math.Max(0, card.CurrentStarCost);
        int cardCost = cardEnergy + cardStars;
        if (cardCost == 0) return;
        
        int playerEnergy = card.Owner.PlayerCombatState?.Energy ?? 0;
        int playerStars = card.Owner.PlayerCombatState?.Stars ?? 0;

        if (card.Pile != PileType.Hand.GetPile(card.Owner) && card.Pile != PileType.Play.GetPile(card.Owner) && 
            cardCost == BaseEnergyCost(card) + Math.Max(0, card.CanonicalStarCost))
        {
            card.EnergyCost.SetThisCombat(BaseEnergyCost(card));
            card.SetStarCostThisCombat(card.CanonicalStarCost);
            return;
        }
        
        if (playerStars + playerEnergy >= cardCost)
        {
            int newEnergyCost = Math.Min(playerEnergy, BaseEnergyCost(card));
            int newStarCost = Math.Min(playerStars, Math.Max(0, card.CanonicalStarCost));
            if (cardCost == BaseEnergyCost(card) + Math.Max(0, card.CanonicalStarCost) &&
                newEnergyCost == BaseEnergyCost(card) && newStarCost == Math.Max(0, card.CanonicalStarCost))
            {
                card.EnergyCost.SetThisCombat(BaseEnergyCost(card));
                card.SetStarCostThisCombat(card.CanonicalStarCost);
                return;
            }

            if (newEnergyCost < BaseEnergyCost(card))
            {
                newStarCost = Math.Max(newStarCost, cardCost - playerEnergy);
            }
            if (newStarCost < card.CanonicalStarCost)
            {
                newEnergyCost = Math.Max(newEnergyCost, cardCost - playerStars);
            }
            if (newEnergyCost + newStarCost != cardCost)
            {
                int dif = cardCost - (newEnergyCost + newStarCost);
                if (dif < 0) return;
                if (playerEnergy >= newEnergyCost + dif)
                {
                    newEnergyCost += dif;
                }
                else if (playerStars >= newStarCost + dif)
                {
                    newStarCost += dif;
                }
                else return;
            }
            if (permanentCostChange)
            {
                card.EnergyCost.SetThisCombat(newEnergyCost);
                card.SetStarCostThisCombat(newStarCost);
            }
            else
            {
                card.EnergyCost.SetThisTurnOrUntilPlayed(newEnergyCost);
                card.SetStarCostThisTurn(newStarCost);
            }
        }
        else if (cardCost == BaseEnergyCost(card) + Math.Max(0, card.CanonicalStarCost))
        {
            card.EnergyCost.SetThisCombat(BaseEnergyCost(card));
            card.SetStarCostThisCombat(card.CanonicalStarCost);
        }
    }
}