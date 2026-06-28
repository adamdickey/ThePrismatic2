using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
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
public class Flatten2() : ThePrismatic2Card(2, 
    CardType.Attack, CardRarity.Common, 
    TargetType.AnyEnemy)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<NecrobinderCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/flatten.png-04012f64bf19cc92b525146f7d426bb1.ctex";
    public override string PortraitPath => "res://.godot/imported/flatten.png-04012f64bf19cc92b525146f7d426bb1.ctex";
    
    protected override bool ShouldGlowGoldInternal => PlayedCostlyCardThisTurn;

    protected override HashSet<CardTag> CanonicalTags => [CardTag.OstyAttack];

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new SummonVar(1m),
        new OstyDamageVar(10m, ValueProp.Move)
        ]);

    private bool PlayedCostlyCardThisTurn => CombatManager.Instance.History.Entries.OfType<CardPlayFinishedEntry>().Any(e => e.HappenedThisTurn(Owner.Creature.CombatState) && e.CardPlay.Card.EnergyCost.GetResolved() + Math.Max(0, e.CardPlay.Card.LastStarsSpent) >= 2);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await OstyCmd.Summon(choiceContext, Owner, DynamicVars.Summon.BaseValue, this);
        if (!Osty.CheckMissingWithAnim(Owner) && Owner.Osty != null)
        {
            await DamageCmd.Attack(DynamicVars.OstyDamage.BaseValue).FromOsty(Owner.Osty, this).Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
                .Execute(choiceContext);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.OstyDamage.UpgradeValueBy(4m);
    }
    
    public override Task AfterCardEnteredCombat(CardModel card)
    {
        if (card != this)
        {
            return Task.CompletedTask;
        }
        if (!PlayedCostlyCardThisTurn)
        {
            return Task.CompletedTask;
        }
        ReduceCost();
        return Task.CompletedTask;
    }

    public override Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (!(cardPlay.Card.EnergyCost.GetResolved() + Math.Max(0, cardPlay.Card.LastStarsSpent) >= 2))
        {
            return Task.CompletedTask;
        }
        ReduceCost();
        return Task.CompletedTask;
    }

    private void ReduceCost()
    {
        EnergyCost.SetThisTurn(0);
    }
}