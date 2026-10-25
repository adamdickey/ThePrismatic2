using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;
using ThePrismatic2.ThePrismatic2Code.Patches;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class SevenStars2() : ThePrismatic2Card(1, 
    CardType.Attack, CardRarity.Rare, 
    TargetType.AllEnemies)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<RegentCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/seven_stars.png-410c481a54e211f8e73bcf56c7f8c7d0.ctex";
    public override string PortraitPath => "res://.godot/imported/seven_stars.png-410c481a54e211f8e73bcf56c7f8c7d0.ctex";

    private const int PrintedStarCost = 7;

    private int _appliedStarCost = -1;

    // Derived from combat history rather than stored on the card: MemberwiseClone shares reference-type fields
    // between the canonical, deck and combat copies, which made the discount leak out of combat.
    public override int CanonicalStarCost => CombatState == null ? PrintedStarCost : Math.Max(0, PrintedStarCost - UniqueOrbTypesChanneled());

    private int UniqueOrbTypesChanneled() => CombatManager.Instance.History.Entries.OfType<OrbChanneledEntry>()
        .Where(e => e.Actor.Player == Owner)
        .Select(e => e.Orb.GetType())
        .Distinct()
        .Count();
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Channeling));
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => new _003C_003Ez__ReadOnlySingleElementList<CardKeyword>(Extensions.Keywords.Starbound);

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new DamageVar(7m, ValueProp.Move),
        new RepeatVar(7)
    ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState != null)
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).WithHitCount(DynamicVars.Repeat.IntValue)
                .FromCard(this)
                .TargetingAllOpponents(CombatState)
                .WithHitFx("vfx/vfx_starry_impact", null, "slash_attack.mp3")
                .SpawningHitVfxOnEachCreature()
                .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
    
    public override Task AfterCardEnteredCombat(CardModel card)
    {
        if (card != this || IsClone)
        {
            return Task.CompletedTask;
        }
        ResetCost();
        return Task.CompletedTask;
    }
    
    public override Task AfterOrbChanneled(PlayerChoiceContext choiceContext, Player player, OrbModel orb)
    {
        if (player != Owner || CanonicalStarCost == _appliedStarCost)
        {
            return Task.CompletedTask;
        }
        ResetCost();
        return Task.CompletedTask;
    }

    // Drop any Starbound energy/star split made for the old total, then let Starbound re-split the new total.
    private void ResetCost()
    {
        _appliedStarCost = CanonicalStarCost;
        EnergyCost.SetThisCombat(EnergyCost.GetWithModifiers(CostModifiers.None));
        SetStarCostThisCombat(_appliedStarCost);
        StarboundSingleton.Refresh(this);
    }
}