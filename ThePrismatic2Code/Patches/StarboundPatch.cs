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
    
    public override Task AfterCardGeneratedForCombat(CardModel card, Player? creator)
    {
        UpdateStarbound(card);
        return Task.CompletedTask;
    }
    
    public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.ResultPile == PileType.Hand)
        {
            UpdateStarbound(cardPlay.Card, true);
        }
        IEnumerable<CardModel> enumerable = PileType.Hand.GetPile(cardPlay.Card.Owner).Cards;
        foreach (CardModel card in enumerable)
        {
            UpdateStarbound(card);
        }
        return Task.CompletedTask;
    }
    
    public override Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        IEnumerable<CardModel> enumerable = PileType.Hand.GetPile(card.Owner).Cards;
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
            if (playerEnergy < cardEnergy)
            {
                if (permanentCostChange)
                {
                    card.EnergyCost.SetThisCombat(playerEnergy);
                    card.SetStarCostThisCombat(cardCost - playerEnergy);
                }
                else
                {
                    card.EnergyCost.SetThisTurnOrUntilPlayed(playerEnergy);
                    card.SetStarCostThisTurn(cardCost - playerEnergy);
                }
            }
            else if (playerStars < cardStars)
            {
                if (permanentCostChange)
                {
                    card.EnergyCost.SetThisCombat(cardCost - playerStars);
                    card.SetStarCostThisCombat(playerStars);
                }
                else
                {
                    card.EnergyCost.SetThisTurnOrUntilPlayed(cardCost - playerStars);
                    card.SetStarCostThisTurn(playerStars);
                }
            }
            else if (cardCost == card.EnergyCost.Canonical + card.CanonicalStarCost && playerEnergy >= card.EnergyCost.Canonical && playerStars >= card.CanonicalStarCost)
            {
                card.EnergyCost.SetThisCombat(card.EnergyCost.Canonical);
                card.SetStarCostThisCombat(card.CanonicalStarCost);
            }
        }
        else if (cardCost == card.EnergyCost.Canonical + card.CanonicalStarCost)
        {
            card.EnergyCost.SetThisCombat(card.EnergyCost.Canonical);
            card.SetStarCostThisCombat(card.CanonicalStarCost);
        }
    }
}