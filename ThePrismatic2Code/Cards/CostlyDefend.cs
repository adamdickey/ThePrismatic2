using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Extensions;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

public class CostlyDefend() : ThePrismatic2Card(2,
    CardType.Skill, CardRarity.None,
    TargetType.Self)
{
    public override string CustomPortraitPath => $"PrismaticDefend.png".BigCardImagePath();
    public override string PortraitPath => $"PrismaticDefend.png".CardImagePath();
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Defend];
    
    public override bool IsBasicStrikeOrDefend => false;
    public override bool GainsBlock => true;
    
    public override bool CanBeGeneratedInCombat => false;

    private bool _costReduced;
    
    protected override bool ShouldGlowGoldInternal => _costReduced;

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new BlockVar(10m, ValueProp.Move));
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
        if (card != this) return Task.CompletedTask;
        bool costlyCardPlayed = CombatManager.Instance.History.CardPlaysFinished.Any(e => e.CardPlay.Card.EnergyCost.GetResolved() + Math.Max(0, e.CardPlay.Card.LastStarsSpent) >= 2 && e.CardPlay.Card.Owner == Owner && e.HappenedThisTurn(CombatState));
        if (!costlyCardPlayed)
        {
            _costReduced = false;
            return Task.CompletedTask;
        }
        EnergyCost.AddThisTurn(-1);
        _costReduced = true;
        return Task.CompletedTask;
    }

    public override Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card != this && cardPlay.Card.Owner == Owner && !_costReduced && cardPlay.Card.EnergyCost.GetResolved() + cardPlay.Card.LastStarsSpent >= 2)
        {
            EnergyCost.AddThisTurn(-1);
            _costReduced = true;
        }
        return Task.CompletedTask;
    }
    
    public override Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player == Owner)
        {
            _costReduced = false;
        }
        return Task.CompletedTask;
    }
}