using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Rampage2() : ThePrismatic2Card(1, 
    CardType.Attack, CardRarity.Uncommon, 
    TargetType.AnyEnemy)
{
    public override string CustomPortraitPath => "res://.godot/imported/rampage.png-41facc0224a8197dabb863d270aff09f.ctex";
    public override string PortraitPath => "res://.godot/imported/rampage.png-41facc0224a8197dabb863d270aff09f.ctex";

    private const string _increaseKey = "Increase";

    public decimal _extraDamageFromPlays;

    protected override IEnumerable<DynamicVar> CanonicalVars => new global::_003C_003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
    {
        new DamageVar(9m, ValueProp.Move),
        new DynamicVar("Increase", 2m)
    });

    public decimal ExtraDamageFromPlays
    {
        get
        {
            return _extraDamageFromPlays;
        }
        set
        {
            AssertMutable();
            _extraDamageFromPlays = value;
        }
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
        if (base.Owner.PlayerCombatState != null)
        {
            base.DynamicVars.Damage.BaseValue += base.DynamicVars["Increase"].BaseValue*base.Owner.PlayerCombatState.Stars;
            ExtraDamageFromPlays += base.DynamicVars["Increase"].BaseValue*base.Owner.PlayerCombatState.Stars;
        }
    }

    protected override void AfterDowngraded()
    {
        base.AfterDowngraded();
        base.DynamicVars.Damage.BaseValue += ExtraDamageFromPlays;
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["Increase"].UpgradeValueBy(1m);
    }
}