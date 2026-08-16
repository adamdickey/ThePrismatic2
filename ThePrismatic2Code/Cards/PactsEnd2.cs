using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class PactsEnd2() : ThePrismatic2Card(0, 
    CardType.Attack, CardRarity.Rare, 
    TargetType.AllEnemies)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<IroncladCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/pacts_end.png-0c8e0fbbd474d5ca7b81ab3fbfbc9fd1.ctex";
    public override string PortraitPath => "res://.godot/imported/pacts_end.png-0c8e0fbbd474d5ca7b81ab3fbfbc9fd1.ctex";
    
    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new DamageVar(17m, ValueProp.Move),
        new CardsVar(3)
    ]);

    protected override bool ShouldGlowGoldInternal => CanDealDamage;

    private bool CanDealDamage => CardPile.GetCards(Owner, PileType.Exhaust).Count() >= DynamicVars.Cards.IntValue;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CanDealDamage)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).TargetingAllOpponents(CombatState!)
                .WithAttackerAnim(Ironclad.GetHeavyAnimIfApplicable(Owner.Character), Ironclad.GetHeavyAttackDelayIfApplicable(Owner.Character))
                .WithHitFx("vfx/vfx_heavy_blunt", null, "heavy_attack.mp3")
                .WithHitVfxSpawnedAtBase()
                .Execute(choiceContext);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(6m);
    }
}