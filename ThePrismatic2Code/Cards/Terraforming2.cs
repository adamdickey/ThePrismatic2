using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using ThePrismatic2.ThePrismatic2Code.Character;
using ThePrismatic2.ThePrismatic2Code.Orbs;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Terraforming2() : ThePrismatic2Card(1, 
    CardType.Skill, CardRarity.Uncommon, 
    TargetType.Self)
{
    public override string CustomPortraitPath => "res://.godot/imported/terraforming.png-d1f4412a453c4771a987281ee547c2c6.ctex";
    public override string PortraitPath => "res://.godot/imported/terraforming.png-d1f4412a453c4771a987281ee547c2c6.ctex";

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new PowerVar<VigorPower>(3m));

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        HoverTipFactory.FromPower<VigorPower>(),
        HoverTipFactory.Static(StaticHoverTip.Channeling),
        HoverTipFactory.FromOrb<MagmaOrb>()
        ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<VigorPower>(Owner.Creature, DynamicVars["VigorPower"].IntValue, Owner.Creature, this);
        await OrbCmd.Channel<MagmaOrb>(choiceContext, Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["VigorPower"].UpgradeValueBy(2m);
    }
}