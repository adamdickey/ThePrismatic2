using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;
using ThePrismatic2.ThePrismatic2Code.Powers;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Uppercut2() : ThePrismatic2Card(2, 
    CardType.Attack, CardRarity.Uncommon, 
    TargetType.AnyEnemy)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<IroncladCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/uppercut.png-9a2ef872349c253bc03c0fda8057e87e.ctex";
    public override string PortraitPath => "res://.godot/imported/uppercut.png-9a2ef872349c253bc03c0fda8057e87e.ctex";

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new DamageVar(10m, ValueProp.Move),
        new DynamicVar("Power", 1m)
    ]);

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        HoverTipFactory.FromPower<WeakPower>(),
        HoverTipFactory.FromPower<VulnerablePower>(),
        HoverTipFactory.FromPower<ExposedPower>()
    ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
        int amount = DynamicVars["Power"].IntValue;
        await PowerCmd.Apply<WeakPower>(choiceContext, cardPlay.Target, amount, Owner.Creature, this);
        await PowerCmd.Apply<VulnerablePower>(choiceContext, cardPlay.Target, amount, Owner.Creature, this);
        await PowerCmd.Apply<ExposedPower>(choiceContext, cardPlay.Target, amount, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Power"].UpgradeValueBy(1m);
    }
}