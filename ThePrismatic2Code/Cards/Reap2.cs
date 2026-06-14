using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Reap2() : ThePrismatic2Card(3, 
    CardType.Attack, CardRarity.Common, 
    TargetType.AnyEnemy)
{
    public override string CustomPortraitPath => "res://.godot/imported/reap.png-e4ffbdae7991268a1d2e5971010912e2.ctex";
    public override string PortraitPath => "res://.godot/imported/reap.png-e4ffbdae7991268a1d2e5971010912e2.ctex";
    
    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new DamageVar(24m, ValueProp.Move));

    public override IEnumerable<CardKeyword> CanonicalKeywords => new _003C_003Ez__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Retain);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(6m);
    }
    
    public override Task AfterCardEnteredCombat(CardModel card)
    {
        UpdateCost();
        return Task.CompletedTask;
    }

    public override Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        UpdateCost();
        return Task.CompletedTask;
    }
    
    public override Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        UpdateCost();
        return Task.CompletedTask;
    }
    
    public override Task AfterCardDiscarded(PlayerChoiceContext choiceContext, CardModel card)
    {
        UpdateCost();
        return Task.CompletedTask;
    }

    private void UpdateCost()
    {
        int retainCards = PileType.Hand.GetPile(Owner).Cards.Count(card => card != this && card.ShouldRetainThisTurn);
        EnergyCost.SetUntilPlayed(EnergyCost.Canonical - retainCards);
    }
}