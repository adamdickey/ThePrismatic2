using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;
using ThePrismatic2.ThePrismatic2Code.Powers;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class MeteorShower2() : ThePrismatic2Card(0, 
    CardType.Attack, CardRarity.Ancient, 
    TargetType.AllEnemies)
{
    public override string CustomPortraitPath => "res://.godot/imported/meteor_shower.png-59c665e6c164aaaa97c2e16f8f75bf0c.ctex";
    public override string PortraitPath => "res://.godot/imported/meteor_shower.png-59c665e6c164aaaa97c2e16f8f75bf0c.ctex";
    
    public override int CanonicalStarCost => 2;

    public override IEnumerable<CardKeyword> CanonicalKeywords => new _003C_003Ez__ReadOnlySingleElementList<CardKeyword>(Extensions.Keywords.Starbound);
    
    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new DamageVar(14m, ValueProp.Move),
        new PowerVar<VulnerablePower>(2m),
        new PowerVar<WeakPower>(2m),
        new DynamicVar("Exposed", 2m)
    ]);

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        HoverTipFactory.FromPower<WeakPower>(),
        HoverTipFactory.FromPower<VulnerablePower>(),
        HoverTipFactory.FromPower<ExposedPower>()
    ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState != null)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).TargetingAllOpponents(CombatState)
                .WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
                .Execute(choiceContext);
            await PowerCmd.Apply<WeakPower>(CombatState.HittableEnemies, DynamicVars.Weak.BaseValue, Owner.Creature,
                this);
            await PowerCmd.Apply<VulnerablePower>(CombatState.HittableEnemies, DynamicVars.Vulnerable.BaseValue,
                Owner.Creature, this);
            await PowerCmd.Apply<ExposedPower>(CombatState.HittableEnemies, DynamicVars["Exposed"].BaseValue,
                Owner.Creature, this);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(7m);
    }
}