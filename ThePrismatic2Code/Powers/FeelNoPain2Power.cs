using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class FeelNoPain2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/feel_no_pain_power.png-46aab79f7cabf284a87233813f0a1ea6.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/feel_no_pain_power.png-46aab79f7cabf284a87233813f0a1ea6.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block));

    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool _)
    {
        if (card.Owner.Creature == Owner && !card.VisualCardPool.IsColorless)
        { 
            await CreatureCmd.GainBlock(Owner, Amount, ValueProp.Unpowered, null);
        }
    }
}
