using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class Hailstorm2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/hailstorm_power.png-67d27760c4785cf432c1cf753482a505.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/hailstorm_power.png-67d27760c4785cf432c1cf753482a505.s3tc.ctex";
    
    	public override PowerType Type => PowerType.Buff;
    
    	public override PowerStackType StackType => PowerStackType.Counter;
    
    	protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Orbs", 3m));
    
	    public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    	{
    		if (side == Owner.Side)
    		{
			    if (Owner.Player is { PlayerCombatState: not null })
			    {
				    int num = Owner.Player.PlayerCombatState.OrbQueue.Orbs.Count;
				    if (num >= DynamicVars["Orbs"].IntValue)
				    {
					    Flash();
					    await CreatureCmd.Damage(choiceContext, CombatState.HittableEnemies, Amount, ValueProp.Unpowered, Owner);
				    }
			    }
		    }
    	}
}
