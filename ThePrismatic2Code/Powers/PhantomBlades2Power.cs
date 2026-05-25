using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class PhantomBlades2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/phantom_blades_power.png-39a3abe25f45ec969a4d85ae9074d79f.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/phantom_blades_power.png-39a3abe25f45ec969a4d85ae9074d79f.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        HoverTipFactory.FromKeyword(CardKeyword.Retain)
    ]);

    public override Task AfterCardEnteredCombat(CardModel card)
    {
        if (card.EnergyCost.Canonical != 0)
        {
            return Task.CompletedTask;
        }

        if (card.Type != CardType.Attack)
        {
            return Task.CompletedTask;
        }
        if (card.Owner != Owner.Player)
        {
            return Task.CompletedTask;
        }
        CardCmd.ApplyKeyword(card, CardKeyword.Retain);
        return Task.CompletedTask;
    }

    public override Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        foreach (CardModel item in Owner.Player.PlayerCombatState.AllCards.Where(c => c.EnergyCost.Canonical == 0 && c.Type == CardType.Attack))
        {
            CardCmd.ApplyKeyword(item, CardKeyword.Retain);
        }
        return Task.CompletedTask;
    }

    public override decimal ModifyDamageAdditive(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (!props.IsPoweredAttack())
        {
            return 0m;
        }
        if (cardSource == null || !(cardSource.EnergyCost.Canonical == 0 && cardSource.Type == CardType.Attack))
        {
            return 0m;
        }
        if (dealer != Owner)
        {
            return 0m;
        }
        int num = CombatManager.Instance.History.CardPlaysFinished.Count(e => e.HappenedThisTurn(CombatState) && e.CardPlay.Card.EnergyCost.Canonical == 0 && e.CardPlay.Card.Type == CardType.Attack && e.CardPlay.Card.Owner.Creature == Owner);
        if (num > 0)
        {
            return 0m;
        }
        return Amount;
    }
}
