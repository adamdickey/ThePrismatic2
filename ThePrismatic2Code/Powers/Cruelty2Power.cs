using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class Cruelty2Power : ThePrismatic2Power
{
    
    public override string CustomPackedIconPath => "res://.godot/imported/cruelty_power.png-c3b0ac0b1ebe0b7948071c31ab756116.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/cruelty_power.png-c3b0ac0b1ebe0b7948071c31ab756116.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<VulnerablePower>());

    public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target == null || target == Owner || !props.IsPoweredAttack())
        {
            return 1m;
        }
        int numDebuffs = target.Powers.Count(power => power.Type == PowerType.Debuff);
        return 1m + numDebuffs*Amount / 100m;

    }
}
