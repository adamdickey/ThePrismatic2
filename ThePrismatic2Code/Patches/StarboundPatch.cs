using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using ThePrismatic2.ThePrismatic2Code.Extensions;

namespace ThePrismatic2.ThePrismatic2Code.Patches;

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
        if (playerStars + playerEnergy >= cardCost)
        {
            if (playerEnergy < cardEnergy && playerEnergy + playerStars >= cardCost)
            {
                card.EnergyCost.SetThisTurnOrUntilPlayed(playerEnergy);
                card.SetStarCostThisTurn(cardCost - playerEnergy);
                card.InvokeEnergyCostChanged();
                return;
            }
            if (playerStars < cardStars && playerStars + playerEnergy >= cardCost)
            {
                card.EnergyCost.SetThisTurnOrUntilPlayed(cardCost - playerStars);
                card.SetStarCostThisTurn(playerStars);
                card.InvokeEnergyCostChanged();
            }
        }
        else
        {
            if (cardCost == card.EnergyCost.Canonical + card.CanonicalStarCost)
            {
                card.EnergyCost.SetThisCombat(card.EnergyCost.Canonical);
                card.SetStarCostThisCombat(card.CanonicalStarCost);
                card.InvokeEnergyCostChanged();
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
}