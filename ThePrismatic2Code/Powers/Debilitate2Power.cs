using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class Debilitate2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/debilitate_power.png-396dd31a8ef39108fe5941f0c4bcd29a.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/debilitate_power.png-396dd31a8ef39108fe5941f0c4bcd29a.s3tc.ctex";
    
    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
	    HoverTipFactory.FromPower<VulnerablePower>(),
	    HoverTipFactory.FromPower<ExposedPower>()
	    ]);
    
	    public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
	    {
		    if (target != Owner || !props.IsPoweredAttack())
		    {
			    return 1m;
		    }
		    VulnerablePower? power = target.GetPower<VulnerablePower>();
		    if (power == null)
		    {
			    return 1m;
		    }
		    decimal vulnMultiplier = power.ModifyDamageMultiplicative(target, amount, props, dealer, cardSource);
		    return (2*vulnMultiplier - 1) / vulnMultiplier;
	    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    	{
    		if (side == Owner.Side)
    		{
    			await PowerCmd.Decrement(this);
    		}
    	}
}
