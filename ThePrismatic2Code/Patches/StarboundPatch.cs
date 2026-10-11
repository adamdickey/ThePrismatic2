using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using ThePrismatic2.ThePrismatic2Code.Extensions;

namespace ThePrismatic2.ThePrismatic2Code.Patches;

public class StarboundSingleton() : CustomSingletonModel(HookType.Combat)
{
    private sealed class Split
    {
        public int BaseEnergy;
        public int BaseStar;
        public int WrittenEnergy;
        public int WrittenStar;
    }

    private readonly Dictionary<CardModel, Split> _splits = new();

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

    public override Task AfterCombatEnd(CombatRoom room)
    {
        _splits.Clear();
        return Task.CompletedTask;
    }

    private void UpdateStarbound(CardModel card, bool permanentCostChange = false)
    {
        if (card.CombatState == null || (!card.Keywords.Contains(Keywords.Starbound) && !card.Keywords.Contains(Keywords.StarboundThisTurn)))
        {
            _splits.Remove(card);
            return;
        }

        int localEnergy = Math.Max(0, card.EnergyCost.GetWithModifiers(CostModifiers.Local));
        int globalEnergy = Math.Max(0, card.EnergyCost.GetWithModifiers(CostModifiers.All));
        int currentStars = Math.Max(0, card.GetStarCostWithModifiers());

        Split split = GetBaseline(card, localEnergy, currentStars);
        int baseEnergy = split.BaseEnergy;
        int baseStar = split.BaseStar;
        int cardCost = baseEnergy + baseStar;
        if (cardCost == 0)
        {
            return;
        }

        int playerEnergy = 0;
        int playerStars = 0;
        if (card.Owner.PlayerCombatState != null)
        {
            playerEnergy = card.Owner.PlayerCombatState.Energy;
            playerStars = card.Owner.PlayerCombatState.Stars;
        }
        if (localEnergy != globalEnergy)
        {
            playerEnergy -= globalEnergy - localEnergy;
        }

        if (card.Pile != PileType.Hand.GetPile(card.Owner) && card.Pile != PileType.Play.GetPile(card.Owner))
        {
            WriteBaseline(card, split);
            return;
        }

        if (playerStars + playerEnergy < cardCost)
        {
            WriteBaseline(card, split);
            return;
        }

        int newEnergyCost = Math.Min(playerEnergy, baseEnergy);
        int newStarCost = Math.Min(playerStars, baseStar);
        if (newEnergyCost == baseEnergy && newStarCost == baseStar)
        {
            WriteBaseline(card, split);
            return;
        }

        if (newEnergyCost < baseEnergy)
        {
            newStarCost = Math.Max(newStarCost, cardCost - playerEnergy);
        }
        if (newStarCost < baseStar)
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
        split.WrittenEnergy = newEnergyCost;
        split.WrittenStar = newStarCost;
    }

    private Split GetBaseline(CardModel card, int localEnergy, int currentStars)
    {
        if (!_splits.TryGetValue(card, out Split? split))
        {
            split = new Split
            {
                BaseEnergy = localEnergy,
                BaseStar = currentStars,
                WrittenEnergy = localEnergy,
                WrittenStar = currentStars
            };
            _splits[card] = split;
            return split;
        }
        if (split.WrittenEnergy != localEnergy)
        {
            split.BaseEnergy = localEnergy;
            split.WrittenEnergy = localEnergy;
        }
        if (split.WrittenStar != currentStars)
        {
            split.BaseStar = currentStars;
            split.WrittenStar = currentStars;
        }
        return split;
    }

    private static void WriteBaseline(CardModel card, Split split)
    {
        card.EnergyCost.SetThisCombat(split.BaseEnergy);
        card.SetStarCostThisCombat(split.BaseStar);
        split.WrittenEnergy = split.BaseEnergy;
        split.WrittenStar = split.BaseStar;
    }
}
