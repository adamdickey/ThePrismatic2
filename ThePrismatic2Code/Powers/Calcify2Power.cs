using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class Calcify2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/calcify_power.png-83482b1c81d634e402c20c2e10f26e20.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/calcify_power.png-83482b1c81d634e402c20c2e10f26e20.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Single;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(Extensions.Keywords.DualWield));
    
    public override decimal ModifyDamageAdditive(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (cardSource == null)
        {
            return 0m;
        }
        if (!cardSource.Keywords.Contains(Extensions.Keywords.DualWield))
        {
            return 0m;
        }
        if (dealer?.Monster is not Osty)
        {
            return 0m;
        }
        if (Owner != dealer.PetOwner?.Creature)
        {
            return 0m;
        }
        if (!props.IsPoweredAttack())
        {
            return 0m;
        }
        return cardSource.DynamicVars.Damage.BaseValue - cardSource.DynamicVars.OstyDamage.BaseValue;
    }
}
