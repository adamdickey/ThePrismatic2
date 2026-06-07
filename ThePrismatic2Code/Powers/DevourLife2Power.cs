using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class DevourLife2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/devour_life_power.png-94ceafbfba62965d1d59ee00aee6e78a.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/devour_life_power.png-94ceafbfba62965d1d59ee00aee6e78a.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.SummonStatic));

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Type == CardType.Skill && cardPlay.Card.EnergyCost.GetResolved() == 0 && cardPlay.Card.Owner.Creature == Owner)
        {
            await OstyCmd.Summon(context, cardPlay.Card.Owner, Amount, this);
        }
    }
}
