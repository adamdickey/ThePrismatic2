using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using ThePrismatic2.ThePrismatic2Code.Character;
using ThePrismatic2.ThePrismatic2Code.Orbs;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class BigBang2() : ThePrismatic2Card(0, 
    CardType.Skill, CardRarity.Rare, 
    TargetType.Self)
{
    public override string CustomPortraitPath => "res://.godot/imported/big_bang.png-b5a7e2010736e5f5b6196406aa0e1bb7.ctex";
    public override string PortraitPath => "res://.godot/imported/big_bang.png-b5a7e2010736e5f5b6196406aa0e1bb7.ctex";

    public override IEnumerable<CardKeyword> CanonicalKeywords => new _003C_003Ez__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new CardsVar(1),
        new EnergyVar(1)
    ]);

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        EnergyHoverTip,
        HoverTipFactory.Static(StaticHoverTip.Channeling),
        HoverTipFactory.FromOrb<SolarOrb>(),
        HoverTipFactory.FromOrb<IronOrb>()
    ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
        await PlayerCmd.GainEnergy(DynamicVars.Energy.IntValue, Owner);
        await OrbCmd.Channel<SolarOrb>(choiceContext, Owner);
        await OrbCmd.Channel<IronOrb>(choiceContext, Owner);
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Innate);
    }
}