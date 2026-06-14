using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class NecroMastery2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/necro_mastery_power.png-b648d13f5e5401dc8d30091fb2913aa0.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/necro_mastery_power.png-b648d13f5e5401dc8d30091fb2913aa0.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;
    
    	public override PowerStackType StackType => PowerStackType.Counter;

	    public override async Task AfterCurrentHpChanged(Creature creature, decimal delta)
	    {
		    if (!(delta >= 0m) && Owner.Player != null && ((creature.Monster is Osty && creature.PetOwner == Owner.Player) || creature == Owner.Player.Creature))
		    {
			    if (creature.CombatState?.HittableEnemies != null)
				    await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), creature.CombatState.HittableEnemies,
					    -delta * Amount, ValueProp.Unblockable | ValueProp.Unpowered, Owner, null);
		    }
	    }
}
