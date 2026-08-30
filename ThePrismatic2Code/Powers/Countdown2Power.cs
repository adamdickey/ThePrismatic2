using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class Countdown2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/countdown_power.png-cb4e1d479b31baa0bd8940d6de6621b6.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/countdown_power.png-cb4e1d479b31baa0bd8940d6de6621b6.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<DoomPower>());
    
    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (participants.Contains(Owner))
        {
            Flash();
            Creature? creature = Owner.Player?.RunState.Rng.CombatTargets.NextItem(CombatState.HittableEnemies);
            if (creature != null)
            {
                decimal doomAmount = Amount * (from orb in Owner.Player?.PlayerCombatState?.OrbQueue.Orbs group orb by orb.Id).Count();
                await PowerCmd.Apply<DoomPower>(new ThrowingPlayerChoiceContext(), creature, doomAmount, Owner, null);
            }
        }
    }
}
