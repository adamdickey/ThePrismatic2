using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

  
public class ExposedPower : ThePrismatic2Power
{

    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("DamageIncrease", 1.5m));

    public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target != Owner)
        {
            return 1m;
        }
        if (props.IsPoweredAttack())
        {
            return 1m;
        }
        decimal num = DynamicVars["DamageIncrease"].BaseValue;
        PowerModel? debilitatePower = target.GetPower<Debilitate2Power>();
        if (debilitatePower != null)
        {
            return 2m;
        }
        return num;
    }
    
    public override decimal ModifyPowerAmountGivenMultiplicative(PowerModel power, Creature giver, decimal amount, Creature? target, CardModel? cardSource)
    {
        if (target == null || !target.Powers.Contains(this))
        {
            return amount;
        }
        if (power is DoomPower)
        {
            PowerModel? debilitatePower = target.GetPower<Debilitate2Power>();
            if (debilitatePower != null)
            {
                return amount * 2m;
            }
            return amount * 1.5m;
        }
        return amount;
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (side == CombatSide.Enemy)
        {
            await PowerCmd.TickDownDuration(this);
        }
    }
}
