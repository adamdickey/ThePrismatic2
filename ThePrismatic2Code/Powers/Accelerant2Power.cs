using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class Accelerant2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/accelerant_power.png-c2535fe4d2cae1d2eee5caaeb7615b10.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/accelerant_power.png-c2535fe4d2cae1d2eee5caaeb7615b10.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<DoomPower>());
    
    public override decimal ModifyPowerAmountGivenMultiplicative(PowerModel power, Creature giver, decimal amount, Creature? target, CardModel? cardSource)
    {
        if (giver == Owner && target != Owner && power is DoomPower)
        {
            return Amount;
        }
        return amount;
    }
}
