using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
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

    protected override bool ShouldGlowGoldInternal => Owner.PlayerCombatState != null && Owner.PlayerCombatState.Stars >= 2*PileType.Draw.GetPile(Owner).Cards.Count;
    public override bool HasStarCostX => true;

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new DamageVar(50m, ValueProp.Move),
        new CalculationBaseVar(0m),
        new CalculationExtraVar(2m),
        new CalculatedVar("StarCost").WithMultiplier((card, _) =>
            card.Owner.PlayerCombatState == null ? 0 : PileType.Draw.GetPile(card.Owner).Cards.Count)
    ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState != null && ResolveStarXValue() >= 2 * PileType.Draw.GetPile(Owner).Cards.Count)
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
}