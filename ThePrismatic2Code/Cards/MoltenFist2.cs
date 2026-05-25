using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;
using ThePrismatic2.ThePrismatic2Code.Powers;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class MoltenFist2() : ThePrismatic2Card(1, 
    CardType.Attack, CardRarity.Common, 
    TargetType.AnyEnemy)
{
    public override string CustomPortraitPath => "res://.godot/imported/molten_fist.png-57a3ab055339fbb3cacf8212d7a62c10.ctex";
    public override string PortraitPath => "res://.godot/imported/molten_fist.png-57a3ab055339fbb3cacf8212d7a62c10.ctex";

    private const string _moltenFistVfxPath = "vfx/vfx_molten_fist";

    protected override IEnumerable<string> ExtraRunAssetPaths => new _003C_003Ez__ReadOnlySingleElementList<string>(SceneHelper.GetScenePath("vfx/vfx_molten_fist"));

    public override IEnumerable<CardKeyword> CanonicalKeywords => new _003C_003Ez__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new DamageVar(8m, ValueProp.Move));

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>(new IHoverTip[3]
    {
        HoverTipFactory.FromPower<VulnerablePower>(),
        HoverTipFactory.FromPower<WeakPower>(),
        HoverTipFactory.FromPower<ExposedPower>()
    });

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_molten_fist", null, "blunt_attack.mp3")
            .Execute(choiceContext);
        int num = (cardPlay.Target.IsAlive ? cardPlay.Target.GetPowerAmount<VulnerablePower>() : 0);
        if (num > 0)
        {
            await PowerCmd.Apply<VulnerablePower>(cardPlay.Target, num, Owner.Creature, this);
        }
        num = (cardPlay.Target.IsAlive ? cardPlay.Target.GetPowerAmount<WeakPower>() : 0);
        if (num > 0)
        {
            await PowerCmd.Apply<WeakPower>(cardPlay.Target, num, Owner.Creature, this);
        }
        num = (cardPlay.Target.IsAlive ? cardPlay.Target.GetPowerAmount<ExposedPower>() : 0);
        if (num > 0)
        {
            await PowerCmd.Apply<ExposedPower>(cardPlay.Target, num, Owner.Creature, this);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4m);
    }
}