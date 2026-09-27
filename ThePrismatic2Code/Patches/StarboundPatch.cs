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

    private static void UpdateStarbound(CardModel card, bool permanentCostChange = false)
    {
        if (card.CombatState == null || (!card.Keywords.Contains(Keywords.Starbound) && !card.Keywords.Contains(Keywords.StarboundThisTurn))) return;
        
        int cardEnergy = Math.Max(0, card.EnergyCost.GetWithModifiers(CostModifiers.Local));
        int globalCardEnergy = Math.Max(0, card.EnergyCost.GetWithModifiers(CostModifiers.All));
        int cardStars = Math.Max(0, card.GetStarCostWithModifiers());
        int cardCost = cardEnergy + cardStars;
        if (cardCost == 0) return;
        
        int playerEnergy = 0;
        int playerStars = 0;
        if (card.Owner.PlayerCombatState != null)
        {
            playerEnergy = card.Owner.PlayerCombatState.Energy;
            playerStars = card.Owner.PlayerCombatState.Stars;
        }
        if (cardEnergy != globalCardEnergy)
        {
            int energyDif =  globalCardEnergy - cardEnergy;
            playerEnergy -= energyDif;
        }
        
        if (playerStars + playerEnergy >= cardCost)
        {
            int newEnergyCost = Math.Min(playerEnergy, card.EnergyCost.Canonical);
            int newStarCost = Math.Min(playerStars, card.CanonicalStarCost);
            if (cardCost == card.EnergyCost.Canonical + Math.Max(0, card.CanonicalStarCost) && newEnergyCost == card.EnergyCost.Canonical && newStarCost == Math.Max(0, card.CanonicalStarCost))
            {
                card.EnergyCost.SetThisCombat(card.EnergyCost.Canonical);
                card.SetStarCostThisCombat(card.CanonicalStarCost);
                return;
            }

            if (newEnergyCost < card.EnergyCost.Canonical)
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
        else if (cardCost == card.EnergyCost.Canonical + Math.Max(0, card.CanonicalStarCost))
        {
            card.EnergyCost.SetThisCombat(card.EnergyCost.Canonical);
            card.SetStarCostThisCombat(card.CanonicalStarCost);
        }
    }
}