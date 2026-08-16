using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class HeavenlyDrill2() : ThePrismatic2Card(0, 
    CardType.Attack, CardRarity.Rare, 
    TargetType.AnyEnemy)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<RegentCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/heavenly_drill.png-3638716372fc9b8acc5e642b52015e3a.ctex";
    public override string PortraitPath => "res://.godot/imported/heavenly_drill.png-3638716372fc9b8acc5e642b52015e3a.ctex";
    
    protected override bool HasEnergyCostX => true;

    protected override bool ShouldGlowGoldInternal => Owner.PlayerCombatState != null && Owner.PlayerCombatState.Energy >= DynamicVars.Energy.IntValue;

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new DamageVar(8m, ValueProp.Move),
        new EnergyVar(4)
    ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        int num = ResolveEnergyXValue();
        if (num >= DynamicVars.Energy.IntValue)
        {
            num *= 2;
        }
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).WithHitCount(num).FromCard(this)
            .Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_giant_horizontal_slash", null, "slash_attack.mp3")
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
    }
}