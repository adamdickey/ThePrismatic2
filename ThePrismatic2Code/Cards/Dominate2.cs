using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using ThePrismatic2.ThePrismatic2Code.Character;
using ThePrismatic2.ThePrismatic2Code.Powers;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Dominate2() : ThePrismatic2Card(1, 
    CardType.Skill, CardRarity.Uncommon, 
    TargetType.AnyEnemy)
{
    public override string CustomPortraitPath => "res://.godot/imported/dominate.png-8bb755cf6f657e51d5fa88a65015f90f.ctex";
    public override string PortraitPath => "res://.godot/imported/dominate.png-8bb755cf6f657e51d5fa88a65015f90f.ctex";

    private const string _strengthPerVulnerableKey = "StrengthPerVulnerable";

    public override IEnumerable<CardKeyword> CanonicalKeywords => new global::_003C_003Ez__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);

    protected override IEnumerable<DynamicVar> CanonicalVars => new global::_003C_003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
    {
        new PowerVar<VulnerablePower>(1m),
        new DynamicVar("StrengthPerVulnerable", 1m)
    });

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new global::_003C_003Ez__ReadOnlyArray<IHoverTip>(new IHoverTip[4]
    {
        HoverTipFactory.FromPower<StrengthPower>(),
        HoverTipFactory.FromPower<WeakPower>(),
        HoverTipFactory.FromPower<VulnerablePower>(),
        HoverTipFactory.FromPower<ExposedPower>()
    });

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<VulnerablePower>(cardPlay.Target, base.DynamicVars["VulnerablePower"].BaseValue, base.Owner.Creature, this);
        int num = (cardPlay.Target.GetPower<VulnerablePower>()?.Amount??0)+(cardPlay.Target.GetPower<WeakPower>()?.Amount??0)+(cardPlay.Target.GetPower<ExposedPower>()?.Amount??0);
        await PowerCmd.Apply<StrengthPower>(base.Owner.Creature, num, base.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["VulnerablePower"].UpgradeValueBy(1m);
    }
}