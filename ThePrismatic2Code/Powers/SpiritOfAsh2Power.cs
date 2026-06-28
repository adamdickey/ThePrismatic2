using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.ValueProps;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class SpiritOfAsh2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/spirit_of_ash_power.png-72df7743edf5f8a61729572d65a8c309.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/spirit_of_ash_power.png-72df7743edf5f8a61729572d65a8c309.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        HoverTipFactory.FromKeyword(CardKeyword.Ethereal),
        HoverTipFactory.FromKeyword(CardKeyword.Exhaust),
        HoverTipFactory.Static(StaticHoverTip.Block)
    ]);

    public override async Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner == Owner.Player && (cardPlay.Card.Keywords.Contains(CardKeyword.Ethereal) || cardPlay.Card.Keywords.Contains(CardKeyword.Exhaust)))
        {
            await CreatureCmd.GainBlock(Owner, Amount, ValueProp.Unpowered, null);
        }
    }
}
