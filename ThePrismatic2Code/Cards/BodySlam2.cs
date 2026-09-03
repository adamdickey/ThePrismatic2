using BaseLib.Cards.Variables;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class BodySlam2() : ThePrismatic2Card(1, 
    CardType.Attack, CardRarity.Common, 
    TargetType.AnyEnemy)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<IroncladCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/body_slam.png-a6564034dd7a8a0880db3ec339d1d71e.ctex";
    public override string PortraitPath => "res://.godot/imported/body_slam.png-a6564034dd7a8a0880db3ec339d1d71e.ctex";
    
    protected override HashSet<CardTag> CanonicalTags => [CardTag.OstyAttack];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new CalculationBaseVar(0m),
        new ExtraDamageVar(1m),
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier((card, _) => card.Owner.Creature.Block),
        new DynamicVar("OstyDamageBase", 0m),
        new DynamicVar("OstyDamageExtra", 1m),
        // Typed on purpose - see Murder2. CustomCalculatedDamageVar hides the inherited CardModel
        // overload of WithMultiplier, so an untyped lambda silently binds to the RelicModel one and
        // stores the delegate where Calculate() never reads it. This line only resolved correctly
        // because .FromOsty() returns the base type first; typing it removes that dependency.
        new CustomCalculatedDamageVar("OstyDamage", ValueProp.Move).FromOsty().WithMultiplier((CardModel card, Creature? _) => card.Owner.Osty?.CurrentHp ?? 0)
    ]);

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block));

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DynamicVars.CalculatedDamage).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
            .Execute(choiceContext);
        if (!Osty.CheckMissingWithAnim(Owner) && Owner.Osty != null)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target);
            await DamageCmd.Attack(((CalculatedVar)DynamicVars["OstyDamage"]).Calculate(cardPlay.Target))
                .FromOsty(Owner.Osty, this)
                .Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
                .Execute(choiceContext);
        }
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}