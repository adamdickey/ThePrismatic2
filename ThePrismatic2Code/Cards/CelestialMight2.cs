using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class CelestialMight2() : ThePrismatic2Card(2, 
    CardType.Attack, CardRarity.Common, 
    TargetType.AnyEnemy)
{
    public override string CustomPortraitPath => "res://.godot/imported/celestial_might.png-b39f250a3144e33e06447f2e1893cd44.ctex";
    public override string PortraitPath => "res://.godot/imported/celestial_might.png-b39f250a3144e33e06447f2e1893cd44.ctex";
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => new _003C_003Ez__ReadOnlySingleElementList<CardKeyword>(Extensions.Keywords.DualWield);

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new DamageVar(5m, ValueProp.Move),
        new OstyDamageVar(2m, ValueProp.Move),
        new RepeatVar(3)
    ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).WithHitCount(DynamicVars.Repeat.IntValue).FromCard(this)
            .Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_starry_impact", null, "blunt_attack.mp3")
            .Execute(choiceContext);
        if (!Osty.CheckMissingWithAnim(Owner))
        {
            await DamageCmd.Attack(DynamicVars.OstyDamage.BaseValue).FromOsty(Owner.Osty, this).WithHitCount(DynamicVars.Repeat.IntValue)
                .Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_blunt")
                .Execute(choiceContext);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Repeat.UpgradeValueBy(1m);
    }
}