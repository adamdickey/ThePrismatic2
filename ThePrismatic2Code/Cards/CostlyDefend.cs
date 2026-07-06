using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Extensions;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

public class CostlyDefend() : ThePrismatic2Card(2,
    CardType.Skill, CardRarity.Basic,
    TargetType.Self)
{
    public override string CustomPortraitPath => $"PrismaticDefend.png".BigCardImagePath();
    public override string PortraitPath => $"PrismaticDefend.png".CardImagePath();
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Defend];
    public override bool GainsBlock => true;

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new BlockVar(9m, ValueProp.Move));
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(Extensions.Keywords.Costly));

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3m);
    }
    
    public override Task AfterCardEnteredCombat(CardModel card)
    {
        UpdateCost();
        return Task.CompletedTask;
    }

    public override Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        UpdateCost();
        return Task.CompletedTask;
    }

    private void UpdateCost()
    {
        int costlyCardsPlayed = CombatManager.Instance.History.CardPlaysFinished.Count(e => e.CardPlay.Card.EnergyCost.GetResolved() + e.CardPlay.Card.LastStarsSpent >= 2 && e.CardPlay.Card.Owner == Owner && e.HappenedThisTurn(CombatState));
        EnergyCost.SetUntilPlayed(EnergyCost.Canonical - Math.Min(1, costlyCardsPlayed));
    }
}