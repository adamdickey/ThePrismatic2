using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;

namespace ThePrismatic2.ThePrismatic2Code.Extensions;

[HarmonyPatch(typeof(PlayerCombatState), "HasEnoughResourcesFor")]
public static class StarboundPatch
{
    private static void Prefix(CardModel card, out UnplayableReason reason)
    {
        reason = UnplayableReason.None;
        int cardEnergy = Math.Max(0, card.EnergyCost.Canonical);
        int cardStars = Math.Max(0, card.CanonicalStarCost);
        int cardCost = cardEnergy + cardStars;
        int playerEnergy = 0;
        int playerStars = 0;
        if (card.Owner.PlayerCombatState != null)
        {
            playerEnergy = card.Owner.PlayerCombatState.Energy;
            playerStars = card.Owner.PlayerCombatState.Stars;
        }

        if (card.CombatState != null && (card.Keywords.Contains(Keywords.Starbound) || card.Keywords.Contains(Keywords.StarboundThisTurn)))
        {
            if (playerEnergy >= cardEnergy && playerStars >= cardStars)
            {
                card.EnergyCost.SetThisTurnOrUntilPlayed(cardEnergy);
                card.SetStarCostThisTurn(cardStars);
                reason = UnplayableReason.None;
                return;
            }
            if (playerEnergy < cardEnergy && playerEnergy + playerStars >= cardCost)
            {
                card.EnergyCost.SetThisTurnOrUntilPlayed(playerEnergy);
                card.SetStarCostThisTurn(cardCost - playerEnergy);
                reason = UnplayableReason.None;
            }
            if (playerStars < cardStars && playerStars + playerEnergy >= cardCost)
            {
                card.EnergyCost.SetThisTurnOrUntilPlayed(cardCost - playerStars);
                card.SetStarCostThisTurn(playerStars);
                reason = UnplayableReason.None;
            }
        }
    }
}