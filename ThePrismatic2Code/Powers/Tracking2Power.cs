using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class Tracking2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/tracking_power.png-c92782dc2d561036e85808d62312bb58.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/tracking_power.png-c92782dc2d561036e85808d62312bb58.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (!props.IsPoweredAttack())
        {
            return 1m;
        }
        if (cardSource == null)
        {
            return 1m;
        }
        if (dealer != Owner && dealer != null && !Owner.Pets.Contains(dealer))
        {
            return 1m;
        }

        if (target == null)
        {
            return 1m;
        }
        bool debuffed = target.Powers.Any(power => power.Type == PowerType.Debuff);
        if (!debuffed)
        {
            return 1m;
        }
        return 1m + Amount*0.01m;
    }
}
