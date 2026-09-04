using MegaCrit.Sts2.Core.Combat;
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

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Retain));
    
    public override bool TryModifyKeywordsInCombat(CardModel card, ISet<CardKeyword> keywords)
    {
        if (card.Owner != Owner.Player)
        {
            return false;
        }
        if (card.EnergyCost.GetWithModifiers(CostModifiers.All) != 0 || card.Type != CardType.Attack)
        {
            return false;
        } 
        return keywords.Add(CardKeyword.Retain);
    }

    public override decimal ModifyDamageAdditive(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (!props.IsPoweredAttack())
        {
            return 0m;
        }
        if (cardSource == null || !(cardSource.EnergyCost.GetWithModifiers(CostModifiers.All) == 0 && cardSource.Type == CardType.Attack))
        {
            return 0m;
        }
        if (Owner != dealer && Owner.Player?.Osty != dealer)
        {
            return 0m;
        }
        int num = CombatManager.Instance.History.CardPlaysFinished.Count(e => e.HappenedThisTurn(CombatState) && e.CardPlay.Resources.EnergyValue == 0 && e.CardPlay.Card.Type == CardType.Attack && e.CardPlay.Card.Owner.Creature == Owner);
        if (num > 0)
        {
            return 0m;
        }
        return Amount;
    }
}
