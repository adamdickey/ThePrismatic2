using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using ThePrismatic2.ThePrismatic2Code.Orbs;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class Furnace2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/furnace_power.png-03f920233ae7eff24fd627d9cc2352ff.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/furnace_power.png-03f920233ae7eff24fd627d9cc2352ff.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    private class Data
    {
        public readonly Dictionary<CardModel, int> PlayedCards = new();
    }
    
    protected override object InitInternalData()
    {
        return new Data();
    }

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        HoverTipFactory.Static(StaticHoverTip.Channeling),
        HoverTipFactory.FromOrb<IronOrb>()
    ]);

    public override async Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    {
        if (side == Owner.Side)
        {
            for (int i = 0; i < Amount; i++)
            {
                if (Owner.Player != null) await OrbCmd.Channel<IronOrb>(new ThrowingPlayerChoiceContext(), Owner.Player);
            }
        }
    }
    
    public override Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature != Owner)
        {
            return Task.CompletedTask;
        }
        if (CombatState.CurrentSide != Owner.Side)
        {
            return Task.CompletedTask;
        }
        GetInternalData<Data>().PlayedCards.Add(cardPlay.Card, 0);
        if (cardPlay.Card.Type == CardType.Power)
        {
            if (Owner.Player != null) OrbCmd.Channel<IronOrb>(new ThrowingPlayerChoiceContext(), Owner.Player);
        }
        return Task.CompletedTask;
    }
    
    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature == Owner && GetInternalData<Data>().PlayedCards.Remove(cardPlay.Card, out var value))
        {
            for (int i = 0; i < value; i++)
            {
                if (Owner.Player != null) await OrbCmd.Channel<IronOrb>(new ThrowingPlayerChoiceContext(), Owner.Player);
            }
        }
    }
}
