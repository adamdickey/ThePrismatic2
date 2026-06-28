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
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class MementoMori2() : ThePrismatic2Card(1, 
    CardType.Attack, CardRarity.Uncommon, 
    TargetType.AnyEnemy)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<SilentCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/memento_mori.png-4c8dcede20456f9750c993f8cca2cfba.ctex";
    public override string PortraitPath => "res://.godot/imported/memento_mori.png-4c8dcede20456f9750c993f8cca2cfba.ctex";

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new CalculationBaseVar(9m),
        new ExtraDamageVar(4m),
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier(delegate(CardModel card, Creature? _)
        {
            int cardsDiscardedThisTurn = CombatManager.Instance.History.Entries.OfType<CardDiscardedEntry>().Count(e => e.HappenedThisTurn(card.CombatState) && e.Card.Owner == card.Owner);
            int cardsExhaustedThisTurn = CombatManager.Instance.History.Entries.OfType<CardExhaustedEntry>().Count(e => e.HappenedThisTurn(card.CombatState) && e.Card.Owner == card.Owner);
            int cardsCreatedThisTurn = CombatManager.Instance.History.Entries.OfType<CardGeneratedEntry>().Count(e => e.HappenedThisTurn(card.CombatState) && e.Card.Owner == card.Owner);
            return cardsDiscardedThisTurn + cardsExhaustedThisTurn + cardsCreatedThisTurn;
        })
    ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        await DamageCmd.Attack(base.DynamicVars.CalculatedDamage).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.CalculationBase.UpgradeValueBy(2m);
        base.DynamicVars.ExtraDamage.UpgradeValueBy(1m);
    }
}