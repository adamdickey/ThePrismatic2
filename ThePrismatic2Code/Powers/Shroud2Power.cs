using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class Shroud2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/shroud_power.png-b95873169dacde0343a5458926f64abb.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/shroud_power.png-b95873169dacde0343a5458926f64abb.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterPowerAmountChanged(PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (applier == Owner && power.Type == PowerType.Debuff)
        {
            await CreatureCmd.GainBlock(Owner, Amount, ValueProp.Unpowered, null);
        }
    }
}
