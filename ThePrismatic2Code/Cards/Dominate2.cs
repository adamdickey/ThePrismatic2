using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using ThePrismatic2.ThePrismatic2Code.Character;
using ThePrismatic2.ThePrismatic2Code.Powers;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Dominate2() : ThePrismatic2Card(1, 
    CardType.Skill, CardRarity.Rare, 
    TargetType.AnyEnemy)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<IroncladCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/dominate.png-8bb755cf6f657e51d5fa88a65015f90f.ctex";
    public override string PortraitPath => "res://.godot/imported/dominate.png-8bb755cf6f657e51d5fa88a65015f90f.ctex";


    public override IEnumerable<CardKeyword> CanonicalKeywords => new _003C_003Ez__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new PowerVar<VulnerablePower>(1m),
        new DynamicVar("StrengthPerVulnerable", 1m)
    ]);

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        HoverTipFactory.FromPower<StrengthPower>(),
        HoverTipFactory.FromPower<VulnerablePower>(),
        HoverTipFactory.FromPower<ExposedPower>()
    ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<VulnerablePower>(choiceContext, cardPlay.Target, DynamicVars["VulnerablePower"].BaseValue, Owner.Creature, this);
        int num = (cardPlay.Target.GetPower<VulnerablePower>()?.Amount??0)+(cardPlay.Target.GetPower<ExposedPower>()?.Amount??0);
        await PowerCmd.Apply<StrengthPower>(choiceContext, Owner.Creature, num, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["VulnerablePower"].UpgradeValueBy(1m);
    }
}