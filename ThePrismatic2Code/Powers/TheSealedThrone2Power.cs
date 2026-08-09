using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class TheSealedThrone2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/the_sealed_throne_power.png-18fc8f50add9d4ba2cb6765101f0dfb8.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/the_sealed_throne_power.png-18fc8f50add9d4ba2cb6765101f0dfb8.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(Extensions.Keywords.Starbound));
    
    public override Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (power is not TheSealedThrone2Power || power.Owner != Owner)
        {
            return Task.CompletedTask;
        }

        IEnumerable<CardModel> enumerable = Owner.Player?.PlayerCombatState?.AllCards ?? Array.Empty<CardModel>();
        foreach (CardModel card in enumerable)
        {
            if (card.Keywords.Contains(Extensions.Keywords.StarboundThisTurn))
            {
                card.RemoveKeyword(Extensions.Keywords.StarboundThisTurn);
            }
            if (!card.Keywords.Contains(Extensions.Keywords.Starbound))
            {
                card.AddKeyword(Extensions.Keywords.Starbound);
            }
        }
        return Task.CompletedTask;
    }

    public override Task AfterCardEnteredCombat(CardModel card)
    {
        if (card.Keywords.Contains(Extensions.Keywords.StarboundThisTurn))
        {
            card.RemoveKeyword(Extensions.Keywords.StarboundThisTurn);
        }
        if (!card.Keywords.Contains(Extensions.Keywords.Starbound))
        {
            card.AddKeyword(Extensions.Keywords.Starbound);
        }
        return Task.CompletedTask;
    }

    public override Task AfterRemoved(Creature oldOwner)
    {
        IEnumerable<CardModel> enumerable = oldOwner.Player?.PlayerCombatState?.AllCards ?? Array.Empty<CardModel>();
        foreach (CardModel item in enumerable)
        {
            if (item.Keywords.Contains(Extensions.Keywords.Starbound) && !item.CanonicalKeywords.Contains(Extensions.Keywords.Starbound))
            {
                item.RemoveKeyword(Extensions.Keywords.Starbound);
            }
        }
        return Task.CompletedTask;
    }
    
    public override async Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner == Owner.Player)
        {
            int cardsPlayedThisTurn = CombatManager.Instance.History.CardPlaysStarted.Count(e => e.Actor == Owner && e.CardPlay.IsFirstInSeries && e.HappenedThisTurn(CombatState));
            if (cardsPlayedThisTurn < Amount)
            {
                Flash();
                await PlayerCmd.GainStars(Amount, Owner.Player);
            }
        }
    }
}
