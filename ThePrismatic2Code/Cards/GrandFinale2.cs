using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class GrandFinale2() : ThePrismatic2Card(0, 
    CardType.Attack, CardRarity.Rare, 
    TargetType.AllEnemies)
{
    public override string CustomPortraitPath => "res://.godot/imported/grand_finale.png-187e80daf7943dd2e51f9a89659de922.ctex";
    public override string PortraitPath => "res://.godot/imported/grand_finale.png-187e80daf7943dd2e51f9a89659de922.ctex";
    
    public override int CanonicalStarCost => 2;

    protected override bool ShouldGlowGoldInternal => Owner.PlayerCombatState != null && Owner.PlayerCombatState.Stars >= 2*PileType.Draw.GetPile(Owner).Cards.Count;

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new DamageVar(50m, ValueProp.Move),
        new StarsVar(2),
        new CalculationBaseVar(0m),
        new CalculationExtraVar(2m),
        new CalculatedVar("StarCost").WithMultiplier((card, _) =>
            card.Owner.PlayerCombatState == null ? 0 : PileType.Draw.GetPile(card.Owner).Cards.Count)
    ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState != null)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).TargetingAllOpponents(CombatState)
                .WithHitFx("vfx/vfx_attack_slash", null, "blunt_attack.mp3")
                .Execute(choiceContext);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(10m);
    }
    
    public override Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        UpdateCost();
        return Task.CompletedTask;
    }
    
    public override Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
    {
        UpdateCost();
        return Task.CompletedTask;
    }
    
    public override Task AfterCardEnteredCombat(CardModel card)
    {
        UpdateCost();
        return Task.CompletedTask;
    }
    
    private void UpdateCost()
    {
        int starCost = 2 * PileType.Draw.GetPile(Owner).Cards.Count;
        SetStarCostThisCombat(starCost);
    }
}