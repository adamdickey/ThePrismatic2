using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using ThePrismatic2.ThePrismatic2Code.Extensions;

namespace ThePrismatic2.ThePrismatic2Code.Patches;

// TEMPORARY diagnostic: delete this file to remove it.
// Logs the cost tiers of every Starbound card in hand so we can tell whether
// "free" (Void Form) is a cost-layer write or a global modifier.
public class StarboundCostDiagnostic() : CustomSingletonModel(HookType.Combat)
{
    private const string Tag = "[starbound-diag]";

    public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        Dump("after-play:" + cardPlay.Card.Id, cardPlay.Card.Owner);
        return Task.CompletedTask;
    }

    public override Task BeforeCardPlayed(CardPlay cardPlay)
    {
        Dump("before-play:" + cardPlay.Card.Id, cardPlay.Card.Owner);
        return Task.CompletedTask;
    }

    public override Task AfterEnergyReset(Player player)
    {
        Dump("energy-reset", player);
        return Task.CompletedTask;
    }

    public override Task AfterStarsGained(int amount, Player gainer)
    {
        Dump("stars-gained:" + amount, gainer);
        return Task.CompletedTask;
    }

    public override Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        Dump("card-drawn:" + card.Id, card.Owner);
        return Task.CompletedTask;
    }

    private static void Dump(string moment, Player player)
    {
        if (player.PlayerCombatState == null)
        {
            return;
        }

        List<CardModel> starbound = PileType.Hand.GetPile(player).Cards
            .Where(c => c.Keywords.Contains(Keywords.Starbound) || c.Keywords.Contains(Keywords.StarboundThisTurn))
            .ToList();
        if (starbound.Count == 0)
        {
            return;
        }

        int voidForm = player.Creature.GetPowerAmount<VoidFormPower>();
        MainFile.Logger.Info($"{Tag} {moment} | playerEnergy={player.PlayerCombatState.Energy} "
                             + $"playerStars={player.PlayerCombatState.Stars} voidForm={voidForm}");

        foreach (CardModel card in starbound)
        {
            MainFile.Logger.Info($"{Tag}   {card.Id}"
                                 + $" | e:none={card.EnergyCost.GetWithModifiers(CostModifiers.None)}"
                                 + $" local={card.EnergyCost.GetWithModifiers(CostModifiers.Local)}"
                                 + $" all={card.EnergyCost.GetWithModifiers(CostModifiers.All)}"
                                 + $" canon={card.EnergyCost.Canonical}"
                                 + $" | s:canon={card.CanonicalStarCost}"
                                 + $" base={card.BaseStarCost}"
                                 + $" cur={card.CurrentStarCost}"
                                 + $" mod={card.GetStarCostWithModifiers()}");
        }
    }
}
