using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using ThePrismatic2.ThePrismatic2Code.Extensions;

namespace ThePrismatic2.ThePrismatic2Code.Patches;

public class StarboundSingleton() : CustomSingletonModel(true, false)
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
    
    public override Task AfterStarsGained(int amount, Player gainer)
    {
        if (amount <= 0) return Task.CompletedTask;
        IEnumerable<CardModel> enumerable = gainer.PlayerCombatState?.AllCards ?? Array.Empty<CardModel>();
        foreach (CardModel card in enumerable)
        {
            UpdateStarbound(card);
        }
        return Task.CompletedTask;
    }
    public override Task AfterStarsSpent(int amount, Player spender)
    {
        if (amount <= 0) return Task.CompletedTask;
        IEnumerable<CardModel> enumerable = spender.PlayerCombatState?.AllCards ?? Array.Empty<CardModel>();
        foreach (CardModel card in enumerable)
        {
            UpdateStarbound(card);
        }
        return Task.CompletedTask;
    }
    public override Task AfterEnergySpent(CardModel cardModel, int amount)
    {
        if (amount <= 0) return Task.CompletedTask;
        IEnumerable<CardModel> enumerable = cardModel.Owner.PlayerCombatState?.AllCards ?? Array.Empty<CardModel>();
        foreach (CardModel card in enumerable)
        {
            UpdateStarbound(card);
        }
        return Task.CompletedTask;
    }
    
    public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        IEnumerable<CardModel> enumerable = cardPlay.Card.Owner.PlayerCombatState?.AllCards ?? Array.Empty<CardModel>();
        foreach (CardModel card in enumerable)
        {
            UpdateStarbound(card);
        }
        return Task.CompletedTask;
    }
    
    public override Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        UpdateStarbound(card);
        return Task.CompletedTask;
    }

    private static void UpdateStarbound(CardModel card)
    {
        int cardEnergy = Math.Max(0, card.EnergyCost.GetWithModifiers(CostModifiers.All));
        int cardStars = Math.Max(0, card.GetStarCostWithModifiers());
        int cardCost = cardEnergy + cardStars;
        int playerEnergy = 0;
        int playerStars = 0;
        if (card.Owner.PlayerCombatState != null)
        {
            playerEnergy = card.Owner.PlayerCombatState.Energy;
            playerStars = card.Owner.PlayerCombatState.Stars;
        }
        if (cardCost != 0 && card.CombatState != null && (card.Keywords.Contains(Keywords.Starbound) || card.Keywords.Contains(Keywords.StarboundThisTurn)))
        {
            if (playerStars + playerEnergy >= cardCost)
            {
                if (playerEnergy < cardEnergy)
                {
                    card.EnergyCost.SetThisTurnOrUntilPlayed(playerEnergy);
                    card.SetStarCostThisTurn(cardCost - playerEnergy);
                }
                else if (playerStars < cardStars)
                {
                    card.EnergyCost.SetThisTurnOrUntilPlayed(cardCost - playerStars);
                    card.SetStarCostThisTurn(playerStars);
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
}
    
/*
[HarmonyPatch(typeof(PlayerCombatState), "HasEnoughResourcesFor")]
public static class StarboundPatch
{
    private static void Prefix(CardModel card, out UnplayableReason reason)
    {
        reason = UnplayableReason.None;
        int cardEnergy = Math.Max(0, card.EnergyCost.GetWithModifiers(CostModifiers.All));
        int cardStars = Math.Max(0, card.GetStarCostWithModifiers());
        int cardCost = cardEnergy + cardStars;
        int playerEnergy = 0;
        int playerStars = 0;
        if (card.Owner.PlayerCombatState != null)
        {
            playerEnergy = card.Owner.PlayerCombatState.Energy;
            playerStars = card.Owner.PlayerCombatState.Stars;
        }

        if (card.CombatState == null || (!card.Keywords.Contains(Keywords.Starbound) && !card.Keywords.Contains(Keywords.StarboundThisTurn))) return;
        if (cardCost == 0) return;
        if (playerStars + playerEnergy >= cardCost)
        {
            if (playerEnergy < cardEnergy && playerEnergy + playerStars >= cardCost)
            {
                card.EnergyCost.SetThisTurnOrUntilPlayed(playerEnergy);
                card.SetStarCostThisTurn(cardCost - playerEnergy);
                return;
            }
            if (playerStars < cardStars && playerStars + playerEnergy >= cardCost)
            {
                card.EnergyCost.SetThisTurnOrUntilPlayed(cardCost - playerStars);
                card.SetStarCostThisTurn(playerStars);
            }
        }
        else
        {
            if (cardCost == card.EnergyCost.Canonical + card.CanonicalStarCost)
            {
                card.EnergyCost.SetThisCombat(card.EnergyCost.Canonical);
                card.SetStarCostThisCombat(card.CanonicalStarCost);
            }
            if (Math.Max(0, card.EnergyCost.GetWithModifiers(CostModifiers.All)) > playerEnergy)
            {
                reason |= UnplayableReason.EnergyCostTooHigh;
            }
            if (Math.Max(0, card.CurrentStarCost) > playerStars)
            {
                reason |= UnplayableReason.StarCostTooHigh;
            }
        }
    }
}*/