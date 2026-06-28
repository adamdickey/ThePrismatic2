using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Dismantle2() : ThePrismatic2Card(1, 
    CardType.Attack, CardRarity.Uncommon, 
    TargetType.AnyEnemy)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<IroncladCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/dismantle.png-77949299fe0992ea6455964b08284672.ctex";
    public override string PortraitPath => "res://.godot/imported/dismantle.png-77949299fe0992ea6455964b08284672.ctex";
    
    protected override HashSet<CardTag> CanonicalTags => [CardTag.OstyAttack];
    protected override bool ShouldGlowGoldInternal => CombatState != null && (CombatState.HittableEnemies.Any(e => e.Powers.Count(power => power.Type == PowerType.Debuff) >= 2) || !Osty.CheckMissingWithAnim(Owner));
    
    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>(
    [
        new DamageVar(6m, ValueProp.Move),
        new OstyDamageVar(3m, ValueProp.Move)
    ]);

    public override IEnumerable<CardKeyword> CanonicalKeywords => new _003C_003Ez__ReadOnlySingleElementList<CardKeyword>(Extensions.Keywords.DualWield);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        int hitCount = !(cardPlay.Target.Powers.Count(power => power.Type == PowerType.Debuff) >= 2) ? 1 : 2;
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).WithHitCount(hitCount).FromCard(this)
            .Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_blunt", null, "heavy_attack.mp3")
            .Execute(choiceContext);
        if (!Osty.CheckMissingWithAnim(Owner) && Owner.Osty != null)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target);
            hitCount = !(cardPlay.Target.Powers.Count(power => power.Type == PowerType.Debuff) >= 2) ? 1 : 2;
            await DamageCmd.Attack(DynamicVars.OstyDamage.BaseValue).FromOsty(Owner.Osty, this)
                .WithHitCount(hitCount)
                .Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
                .Execute(choiceContext);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
        DynamicVars.OstyDamage.UpgradeValueBy(1m);
    }
}