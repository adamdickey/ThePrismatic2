using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;
using ThePrismatic2.ThePrismatic2Code.Powers;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Tremble2() : ThePrismatic2Card(1, 
    CardType.Skill, CardRarity.Common, 
    TargetType.AnyEnemy)
{
    public override string CustomPortraitPath => "res://.godot/imported/tremble.png-60c5361f3fd50a991fd54859433d2a2f.ctex";
    public override string PortraitPath => "res://.godot/imported/tremble.png-60c5361f3fd50a991fd54859433d2a2f.ctex";

    public override IEnumerable<CardKeyword> CanonicalKeywords => new global::_003C_003Ez__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);

    protected override IEnumerable<DynamicVar> CanonicalVars => new global::_003C_003Ez__ReadOnlyArray<DynamicVar>(
        new DynamicVar[2]
        {
            new PowerVar<VulnerablePower>(2m),
            new DynamicVar("Exposed", 2m)
        });

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new global::_003C_003Ez__ReadOnlyArray<IHoverTip>(
        new IHoverTip[2]
        {
            HoverTipFactory.FromPower<VulnerablePower>(),
            HoverTipFactory.FromPower<ExposedPower>()
        });

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<VulnerablePower>(cardPlay.Target, base.DynamicVars.Vulnerable.BaseValue, base.Owner.Creature, this);
        await PowerCmd.Apply<ExposedPower>(cardPlay.Target, base.DynamicVars["Exposed"].BaseValue, base.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Vulnerable.UpgradeValueBy(1m);
        base.DynamicVars["Exposed"].UpgradeValueBy(1m);
    }
}