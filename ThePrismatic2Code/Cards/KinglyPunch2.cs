using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class KinglyPunch2() : ThePrismatic2Card(1, 
    CardType.Attack, CardRarity.Uncommon, 
    TargetType.AnyEnemy)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<RegentCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/kingly_punch.png-bfbe779c7e14356cbd01448a01163db0.ctex";
    public override string PortraitPath => "res://.godot/imported/kingly_punch.png-bfbe779c7e14356cbd01448a01163db0.ctex";
    
    protected override bool ShouldGlowGoldInternal => !Osty.CheckMissingWithAnim(Owner);
    protected override HashSet<CardTag> CanonicalTags => [CardTag.OstyAttack];
    public override IEnumerable<CardKeyword> CanonicalKeywords => new _003C_003Ez__ReadOnlySingleElementList<CardKeyword>(Extensions.Keywords.DualWield);

    private decimal _extraDamage;

    private decimal ExtraDamage
    {
        get
        {
            return _extraDamage;
        }
        set
        {
            AssertMutable();
            _extraDamage = value;
        }
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new DamageVar(8m, ValueProp.Move),
        new OstyDamageVar(4m, ValueProp.Move),
        new DynamicVar("Increase", 3m)
    ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
        if (!Osty.CheckMissingWithAnim(Owner) && Owner.Osty != null)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target);
            await DamageCmd.Attack(DynamicVars.OstyDamage.BaseValue).FromOsty(Owner.Osty, this).Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_blunt")
                .Execute(choiceContext);
        }
    }

    public override Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if (card != this)
        {
            return Task.CompletedTask;
        }
        decimal baseValue = DynamicVars["Increase"].BaseValue;
        DynamicVars.Damage.BaseValue += baseValue;
        ExtraDamage += baseValue;
        DynamicVars.OstyDamage.BaseValue = DynamicVars.Damage.BaseValue / 2;
        return Task.CompletedTask;
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
        DynamicVars.OstyDamage.UpgradeValueBy(1m);
        DynamicVars["Increase"].UpgradeValueBy(2m);
    }

    protected override void AfterDowngraded()
    {
        base.AfterDowngraded();
        DynamicVars.Damage.BaseValue += ExtraDamage;
        DynamicVars.OstyDamage.BaseValue = DynamicVars.Damage.BaseValue / 2;
    }
}