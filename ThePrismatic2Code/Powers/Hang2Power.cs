using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Cards;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class Hang2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/hang_power.png-c0d3c9aff4f9998705eda6230e8f956e.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/hang_power.png-c0d3c9aff4f9998705eda6230e8f956e.s3tc.ctex";
    
    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<Hang2>());

    public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target != Owner)
        {
            return 1m;
        }
        if (cardSource is not Hang && cardSource is not Hang2)
        {
            return 1m;
        }
        return Amount;
    }
}
