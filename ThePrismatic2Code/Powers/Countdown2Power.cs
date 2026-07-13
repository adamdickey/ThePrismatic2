using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class Countdown2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/countdown_power.png-cb4e1d479b31baa0bd8940d6de6621b6.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/countdown_power.png-cb4e1d479b31baa0bd8940d6de6621b6.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new PowerVar<PoisonPower>(0m));

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        HoverTipFactory.FromPower<PoisonPower>(),
        HoverTipFactory.FromPower<DoomPower>()
    ]);
    
    public override Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (power == this)
        {
            DynamicVars.Poison.UpgradeValueBy(2m);
        }
        return Task.CompletedTask;
    }

    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (side == Owner.Side)
        {
            Flash();
            Creature? creature = Owner.Player?.RunState.Rng.CombatTargets.NextItem(CombatState.HittableEnemies);
            if (creature != null)
            {
                await PowerCmd.Apply<PoisonPower>(new ThrowingPlayerChoiceContext(), creature, DynamicVars.Poison.BaseValue, Owner, null);
                await PowerCmd.Apply<DoomPower>(new ThrowingPlayerChoiceContext(), creature, Amount, Owner, null);
            }
        }
    }
}
