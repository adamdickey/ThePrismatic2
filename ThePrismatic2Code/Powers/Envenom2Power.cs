using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class Envenom2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/envenom_power.png-ceb4a2f333a3eb6153c2e379f1793bd3.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/envenom_power.png-ceb4a2f333a3eb6153c2e379f1793bd3.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new PowerVar<DoomPower>(0m));

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        HoverTipFactory.FromPower<PoisonPower>(),
        HoverTipFactory.FromPower<DoomPower>()
    ]);
    
    public override Task AfterPowerAmountChanged(PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (power == this)
        {
            DynamicVars.Doom.UpgradeValueBy(1m);
        }
        return Task.CompletedTask;
    }

    public override async Task AfterDamageGiven(PlayerChoiceContext choiceContext, Creature? dealer, DamageResult result, ValueProp props, Creature target, CardModel? cardSource)
    {
        if (dealer == Owner && props.IsPoweredAttack() && result.UnblockedDamage > 0)
        {
            await PowerCmd.Apply<PoisonPower>(target, Amount, Owner, null);
            await PowerCmd.Apply<DoomPower>(target, DynamicVars.Doom.BaseValue, Owner, null);
        }
    }
}
