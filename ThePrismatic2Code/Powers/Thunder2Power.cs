using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class Thunder2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/thunder_power.png-bfc2745d9754d8c965dfa8b215fac7c6.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/thunder_power.png-bfc2745d9754d8c965dfa8b215fac7c6.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Evoke));

    public override async Task AfterOrbEvoked(PlayerChoiceContext choiceContext, OrbModel orb, IEnumerable<Creature> targets)
    {
        if (orb.Owner == Owner.Player)
        {
            Creature? creature = Owner.Player.RunState.Rng.CombatTargets.NextItem(CombatState.HittableEnemies);
            if (creature != null)
            {
                Flash();
                SfxCmd.Play("slash_attack.mp3");
                VfxCmd.PlayOnCreatureCenter(creature, "vfx/vfx_attack_slash");
                await CreatureCmd.TriggerAnim(orb.Owner.Creature, "Attack", Owner.Player.Character.AttackAnimDelay);
                await CreatureCmd.Damage(choiceContext, creature, Amount, ValueProp.Unpowered, Owner, null);
            }
        }
    }
}
