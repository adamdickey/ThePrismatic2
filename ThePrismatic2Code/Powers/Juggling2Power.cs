using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class Juggling2Power : ThePrismatic2Power
{
    
    public override string CustomPackedIconPath => "res://.godot/imported/juggling_power.png-f62e788497e9c23c59f372f6772434a7.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/juggling_power.png-f62e788497e9c23c59f372f6772434a7.s3tc.ctex";
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(Extensions.Keywords.Cunning));
    
    private class Data
    {
        public int AttacksPlayedThisTurn;
    }

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override object InitInternalData()
    {
        return new Data();
    }

    public override Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        GetInternalData<Data>().AttacksPlayedThisTurn = CombatManager.Instance.History.CardPlaysStarted.Count(e => e.CardPlay.Card.Type == CardType.Attack && e.CardPlay.Card.Owner.Creature == Owner && e.HappenedThisTurn(CombatState));
        return Task.CompletedTask;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner.Player || cardPlay.Card.Type != CardType.Attack)
        {
            return;
        }
        GetInternalData<Data>().AttacksPlayedThisTurn++;
        if (GetInternalData<Data>().AttacksPlayedThisTurn == 3)
        {
            Flash();
            for (int i = 0; i < Amount; i++)
            {
                CardModel card = cardPlay.Card.CreateClone();
                CardCmd.ApplyKeyword(card, Extensions.Keywords.Cunning);
                await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner.Player);
            }
        }
    }

    public override Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (side == Owner.Side)
        {
            GetInternalData<Data>().AttacksPlayedThisTurn = 0;
        }
        return Task.CompletedTask;
    }
}
