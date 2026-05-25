using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class Juggling2Power : ThePrismatic2Power
{
    
    public override string CustomPackedIconPath => "res://.godot/imported/juggling_power.png-f62e788497e9c23c59f372f6772434a7.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/juggling_power.png-f62e788497e9c23c59f372f6772434a7.s3tc.ctex";
    
    private class Data
    {
        public int attacksPlayedThisTurn;
        public int powersPlayedThisTurn;
    }

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override object InitInternalData()
    {
        return new Data();
    }

    public override Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        GetInternalData<Data>().attacksPlayedThisTurn = CombatManager.Instance.History.CardPlaysStarted.Count((CardPlayStartedEntry e) => e.CardPlay.Card.Type == CardType.Attack && e.CardPlay.Card.Owner.Creature == Owner && e.HappenedThisTurn(CombatState));
        GetInternalData<Data>().powersPlayedThisTurn = CombatManager.Instance.History.CardPlaysStarted.Count((CardPlayStartedEntry e) => e.CardPlay.Card.Type == CardType.Power && e.CardPlay.Card.Owner.Creature == Owner && e.HappenedThisTurn(CombatState));
        return Task.CompletedTask;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner.Player || (cardPlay.Card.Type != CardType.Attack && cardPlay.Card.Type != CardType.Power))
        {
            return;
        }

        if (cardPlay.Card.Type == CardType.Attack)
        {
            GetInternalData<Data>().attacksPlayedThisTurn++;
        }
        else
        {
            GetInternalData<Data>().powersPlayedThisTurn = CombatManager.Instance.History.CardPlaysStarted.Count((CardPlayStartedEntry e) => e.CardPlay.Card.Type == CardType.Power && e.CardPlay.Card.Owner.Creature == Owner && e.HappenedThisTurn(CombatState));
        }
        if (GetInternalData<Data>().attacksPlayedThisTurn + GetInternalData<Data>().powersPlayedThisTurn == 3)
        {
            Flash();
            for (int i = 0; i < Amount; i++)
            {
                CardModel card = cardPlay.Card.CreateClone();
                await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, addedByPlayer: true);
            }
        }
    }

    public override Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side == Owner.Side)
        {
            GetInternalData<Data>().attacksPlayedThisTurn = 0;
            GetInternalData<Data>().powersPlayedThisTurn = 0;
        }
        return Task.CompletedTask;
    }
}
