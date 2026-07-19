using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class DanseMacabre2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/danse_macabre_power.png-3f22a04bc39870d33625a276174cc718.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/danse_macabre_power.png-3f22a04bc39870d33625a276174cc718.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new EnergyVar(2));

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(Extensions.Keywords.Costly));

    public override async Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature == Owner && cardPlay.Card.EnergyCost.GetResolved() + Math.Max(0, cardPlay.Card.LastStarsSpent) >= DynamicVars.Energy.IntValue)
        {
            Flash();
            await CreatureCmd.GainBlock(Owner, Amount, ValueProp.Unpowered, null);
        }
    }
}
