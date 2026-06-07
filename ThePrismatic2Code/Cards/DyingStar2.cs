using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class DyingStar2() : ThePrismatic2Card(1, 
    CardType.Attack, CardRarity.Rare, 
    TargetType.AllEnemies)
{
    public override string CustomPortraitPath => "res://.godot/imported/dying_star.png-7aa2b3fc590171379baa920ccb20939a.ctex";
    public override string PortraitPath => "res://.godot/imported/dying_star.png-7aa2b3fc590171379baa920ccb20939a.ctex";

    public override int CanonicalStarCost => 3;

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new DamageVar(9m, ValueProp.Move),
        new DynamicVar("StrengthLoss", 9m)
    ]);

    public override IEnumerable<CardKeyword> CanonicalKeywords => new _003C_003Ez__ReadOnlyArray<CardKeyword>([
        CardKeyword.Ethereal,
        Extensions.Keywords.Starbound
        ]);

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<StrengthPower>());

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Attack", Owner.Character.AttackAnimDelay);
        if (CombatState != null)
        {
            IReadOnlyList<Creature> enemies = CombatState.HittableEnemies;
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).TargetingAllOpponents(CombatState)
                .WithHitFx("vfx/vfx_starry_impact")
                .SpawningHitVfxOnEachCreature()
                .Execute(choiceContext);
            foreach (Creature enemy in enemies)
            {
                await PowerCmd.Apply<DyingStarPower>(enemy, DynamicVars["StrengthLoss"].BaseValue, Owner.Creature, this);
                VfxCmd.PlayOnCreature(enemy, "vfx/vfx_attack_slash");
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
        DynamicVars["StrengthLoss"].UpgradeValueBy(2m);
    }
}