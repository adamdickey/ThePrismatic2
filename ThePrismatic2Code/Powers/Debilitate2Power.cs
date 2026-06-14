using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class Debilitate2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/debilitate_power.png-396dd31a8ef39108fe5941f0c4bcd29a.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/debilitate_power.png-396dd31a8ef39108fe5941f0c4bcd29a.s3tc.ctex";
    
    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<ExposedPower>());
    
    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    	{
    		if (side == Owner.Side)
    		{
    			await PowerCmd.Decrement(this);
    		}
    	}
}
