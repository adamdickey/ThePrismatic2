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

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class SevenStars2() : ThePrismatic2Card(3, 
    CardType.Attack, CardRarity.Rare, 
    TargetType.AllEnemies)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<RegentCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/seven_stars.png-410c481a54e211f8e73bcf56c7f8c7d0.ctex";
    public override string PortraitPath => "res://.godot/imported/seven_stars.png-410c481a54e211f8e73bcf56c7f8c7d0.ctex";

    public override int CanonicalStarCost => 7;

    private List<Type> _orbsChanneled = new();
    
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
        EnergyCost.UpgradeBy(-2);
    }
    
    public override Task AfterCardEnteredCombat(CardModel card)
    {
        if (card != this)
        {
            return Task.CompletedTask;
        }
        if (IsClone)
        {
            return Task.CompletedTask;
        }
        foreach (OrbChanneledEntry orb in CombatManager.Instance.History.Entries.OfType<OrbChanneledEntry>())
        {
            Type orbType = orb.Orb.GetType();
            if (!_orbsChanneled.Contains(orbType))
            {
                _orbsChanneled.Add(orbType);
            }
        }
        SetStarCostThisCombat(CurrentStarCost - _orbsChanneled.Count);
        return Task.CompletedTask;
    }
    
    public override Task AfterOrbChanneled(PlayerChoiceContext choiceContext, Player player, OrbModel orb)
    {
        Type orbType = orb.GetType();
        foreach (Type orbChanneled in _orbsChanneled)
        {
            if (orbType == orbChanneled)
            {
                return Task.CompletedTask;
            }
        }
        _orbsChanneled.Add(orbType);
        SetStarCostThisCombat(CurrentStarCost - 1);
        return Task.CompletedTask;
    }
}