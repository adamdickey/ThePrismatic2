using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class Accuracy2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/accuracy_power.png-623ec981388bc5dc6d2a1b121d1cea47.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/accuracy_power.png-623ec981388bc5dc6d2a1b121d1cea47.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override decimal ModifyDamageAdditive(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? card)
    {
        if (Owner != dealer && Owner.Player?.Osty != dealer)
        {
            return 0m;
        }
        if (!props.IsPoweredAttack())
        {
            return 0m;
        }
        if (card == null)
        {
            return 0m;
        }
        if (card.EnergyCost.GetWithModifiers(CostModifiers.All) != 0 || card.HasStarCostX || card.EnergyCost.CostsX)
        {
            return 0m;
        }
        return Amount;
    }
}
