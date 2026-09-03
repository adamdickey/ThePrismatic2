using BaseLib.Cards.Variables;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
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
public class Murder2() : ThePrismatic2Card(3, 
    CardType.Attack, CardRarity.Rare, 
    TargetType.AnyEnemy)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<SilentCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/murder.png-07795a0909a23cec9ba29d94b8906dba.ctex";
    public override string PortraitPath => "res://.godot/imported/murder.png-07795a0909a23cec9ba29d94b8906dba.ctex";
    
    protected override bool ShouldGlowGoldInternal => !Osty.CheckMissingWithAnim(Owner);
    protected override HashSet<CardTag> CanonicalTags => [CardTag.OstyAttack];
    public override IEnumerable<CardKeyword> CanonicalKeywords => new _003C_003Ez__ReadOnlySingleElementList<CardKeyword>(Extensions.Keywords.DualWield);
    
    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new CalculationBaseVar(1m),
        new ExtraDamageVar(1m),
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier((card, _) => CombatManager.Instance.History.Entries.OfType<CardDrawnEntry>().Count(e => e.Actor == card.Owner.Creature)),
        new DynamicVar("OstyDamageBase", 0m),
        new DynamicVar("OstyDamageExtra", 1m),
        // The lambda parameters are typed on purpose. CustomCalculatedDamageVar declares its own
        // WithMultiplier overloads (RelicModel, PowerModel) which hide the inherited CardModel one,
        // so an untyped lambda binds to the RelicModel version - it compiles, but stores the
        // delegate where Calculate() never looks, and the var then throws when anything reads it.
        new CustomCalculatedDamageVar("OstyDamage", ValueProp.Move).WithMultiplier((CardModel card, Creature? _) => (CombatManager.Instance.History.Entries.OfType<CardDrawnEntry>().Count(e => e.Actor == card.Owner.Creature)+1)/2)
    ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DynamicVars.CalculatedDamage).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
        if (!Osty.CheckMissingWithAnim(Owner) && Owner.Osty != null)
        {
            await DamageCmd.Attack(((CalculatedVar)DynamicVars["OstyDamage"]).Calculate(cardPlay.Target))
                .FromOsty(Owner.Osty, this)
                .Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
        }
        
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}