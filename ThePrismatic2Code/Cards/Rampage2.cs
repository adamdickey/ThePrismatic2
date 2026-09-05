using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Rampage2() : ThePrismatic2Card(1,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<IroncladCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/rampage.png-41facc0224a8197dabb863d270aff09f.ctex";
    public override string PortraitPath => "res://.godot/imported/rampage.png-41facc0224a8197dabb863d270aff09f.ctex";

    protected override bool ShouldGlowGoldInternal => !Osty.CheckMissingWithAnim(Owner);
    protected override HashSet<CardTag> CanonicalTags => [CardTag.OstyAttack];
    public override IEnumerable<CardKeyword> CanonicalKeywords => new _003C_003Ez__ReadOnlySingleElementList<CardKeyword>(Extensions.Keywords.DualWield);

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new DamageVar(8m, ValueProp.Move),
        new OstyDamageVar(4m, ValueProp.Move),
        new DynamicVar("Increase", 4m),
        // How much growth has been added during the current combat. Kept as a DynamicVar rather
        // than a field so it is cloned, saved and restored with the rest of the card's state.
        // Nothing in the card text refers to it.
        new DynamicVar("Bonus", 0m)
    ]);

    /// <summary>Osty always hits for half of whatever this card is currently dealing.</summary>
    private void SyncOstyDamage() => DynamicVars.OstyDamage.BaseValue = DynamicVars.Damage.BaseValue / 2;

    /// <summary>
    /// The growth only lasts "this combat", so hand back everything accumulated last fight when
    /// the card enters a new one.
    /// </summary>
    public override Task AfterCardEnteredCombat(CardModel card)
    {
        if (card != this) return Task.CompletedTask;

        DynamicVars.Damage.BaseValue -= DynamicVars["Bonus"].BaseValue;
        DynamicVars["Bonus"].BaseValue = 0m;
        SyncOstyDamage();

        return Task.CompletedTask;
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        // Resolved once and reused: CheckMissingWithAnim plays the "no Osty" animation as a
        // side effect, so calling it again for the growth check would play it twice.
        Creature? osty = Osty.CheckMissingWithAnim(Owner) ? null : Owner.Osty;

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        if (osty != null)
        {
            // Sync before the hit so Osty mirrors the damage just dealt, not last play's.
            SyncOstyDamage();
            await DamageCmd.Attack(DynamicVars.OstyDamage.BaseValue).FromOsty(osty, this).Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_blunt")
                .Execute(choiceContext);
        }

        // With Osty out, the card grows twice as fast.
        decimal increase = DynamicVars["Increase"].BaseValue * (osty != null ? 2m : 1m);

        DynamicVars.Damage.BaseValue += increase;
        DynamicVars["Bonus"].BaseValue += increase;
        SyncOstyDamage();
    }

    protected override void OnUpgrade()
    {
        // Damage stays at 8; only the growth per play improves. Osty is derived from
        // Damage, so it needs no upgrade of its own.
        DynamicVars["Increase"].UpgradeValueBy(4m);
    }
}
