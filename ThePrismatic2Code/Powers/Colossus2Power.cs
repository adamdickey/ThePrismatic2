using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class Colossus2Power : ThePrismatic2Power
{
    
    public override string CustomPackedIconPath => "res://.godot/imported/colossus_power.png-8f38438c443223e1fe3e0948a33ded77.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/colossus_power.png-8f38438c443223e1fe3e0948a33ded77.s3tc.ctex";

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("DamageDecrease", 10m));

    public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target != Owner)
        {
            return 1m;
        }
        if (!props.IsPoweredAttack())
        {
            return 1m;
        }
        if (dealer == null)
        {
            return 1m;
        }
        int numDebuffs = dealer.Powers.Count(power => power.TypeForCurrentAmount == PowerType.Debuff);
        return Math.Max(0m, 1m - 0.01m*DynamicVars["DamageDecrease"].BaseValue*numDebuffs);
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (side == CombatSide.Enemy)
        {
            await PowerCmd.TickDownDuration(this);
        }
    }
}
