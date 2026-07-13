using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Radiate2() : ThePrismatic2Card(0,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AllEnemies)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<RegentCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/radiate.png-548394d96bad9ef77cb8566190d9201e.ctex";

    public override string PortraitPath => "res://.godot/imported/radiate.png-548394d96bad9ef77cb8566190d9201e.ctex";

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>(
    [
        new DamageVar(3m, ValueProp.Move),
            new StarsVar(1),
            new CalculationBaseVar(0m),
            new CalculationExtraVar(1m),
            new CalculatedVar("CalculatedHits").WithMultiplier((card, _) => (from e in CombatManager.Instance.History.Entries.OfType<StarsModifiedEntry>()
                where e.HappenedThisTurn(card.CombatState) && e.Amount > 0 && e.Actor == card.Owner.Creature
                select e).Sum(e => e.Amount) + (from e in CombatManager.Instance.History.Entries.OfType<PowerReceivedEntry>()
                where e.HappenedThisTurn(card.CombatState) && e.Amount > 0 && e.Power.Type == PowerType.Debuff && e.Applier == card.Owner.Creature
                select e).Count())
    ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState != null)
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .WithHitCount((int)((CalculatedVar)DynamicVars["CalculatedHits"]).Calculate(cardPlay.Target))
                .FromCard(this)
                .TargetingAllOpponents(CombatState)
                .WithHitFx("vfx/vfx_starry_impact", null, "slash_attack.mp3")
                .SpawningHitVfxOnEachCreature()
                .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1m);
    }
}