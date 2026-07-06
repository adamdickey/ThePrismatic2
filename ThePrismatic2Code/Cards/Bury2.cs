using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Bury2() : ThePrismatic2Card(4, 
    CardType.Attack, CardRarity.Uncommon, 
    TargetType.AnyEnemy)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<NecrobinderCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/bury.png-4e55dd62e5542c147caa2602847c0785.ctex";
    public override string PortraitPath => "res://.godot/imported/bury.png-4e55dd62e5542c147caa2602847c0785.ctex";
    
    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new CalculationBaseVar(48m),
        new ExtraDamageVar(10m),
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier((card, _) => PileType.Hand.GetPile(card.Owner).Cards.Count(c => c.EnergyCost.GetWithModifiers(CostModifiers.All) + Math.Max(0, c.CurrentStarCost) >= 2) - 1)
    ]);

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(Extensions.Keywords.Costly));

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DynamicVars.CalculatedDamage).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.CalculationBase.UpgradeValueBy(8m);
        DynamicVars.ExtraDamage.UpgradeValueBy(2m);
    }
    /*
    public override Task AfterCardEnteredCombat(CardModel card)
    {
        UpdateDamage();
        return Task.CompletedTask;
    }

    public override Task AfterCardDrawn(PlayerChoiceContext context, CardModel card, bool fromHandDraw)
    {
        UpdateDamage();
        return Task.CompletedTask;
    }
    
    public override Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        UpdateDamage();
        return Task.CompletedTask;
    }
    
    public override Task AfterCardDiscarded(PlayerChoiceContext choiceContext, CardModel card)
    {
        UpdateDamage();
        return Task.CompletedTask;
    }
    
    public override Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
    {
        UpdateDamage();
        return Task.CompletedTask;
    }

    private void UpdateDamage()
    {
        int otherCostlyCards = PileType.Hand.GetPile(Owner).Cards.Count(card => card != this && (card is Bury2 || card.EnergyCost.GetResolved() + Math.Max(0, card.CurrentStarCost) >= 2));
        
    }
    */
}