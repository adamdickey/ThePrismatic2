using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Orbs;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class FlameBarrier2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/flame_barrier_power.png-38c3d25824bc2300fd9190d0c190c71a.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/flame_barrier_power.png-38c3d25824bc2300fd9190d0c190c71a.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult _, ValueProp props, Creature? dealer, CardModel? __)
    {
        if (target == Owner && props.IsPoweredAttack())
        {
            for (int i=0; i < Amount; i++)
            {
                if (Owner.Player != null) await OrbCmd.Channel<MagmaOrb>(choiceContext, Owner.Player);
            }
        }
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (Owner.Side != side)
        {
            await PowerCmd.Remove(this);
        }
    }
}
