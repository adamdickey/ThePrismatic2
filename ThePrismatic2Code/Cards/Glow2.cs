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
public class Glow2() : ThePrismatic2Card(1, 
    CardType.Skill, CardRarity.Common, 
    TargetType.Self)
{
    public override string CustomPortraitPath => "res://.godot/imported/glow.png-6313250522270908307d1c7beb40dd68.ctex";
    public override string PortraitPath => "res://.godot/imported/glow.png-6313250522270908307d1c7beb40dd68.ctex";

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new CardsVar(1)
    ]);
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>(new IHoverTip[2]
    {
        HoverTipFactory.Static(StaticHoverTip.Channeling),
        HoverTipFactory.FromOrb<SolarOrb>()
    });

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await OrbCmd.Channel<SolarOrb>(choiceContext, Owner);
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1m);
    }
}