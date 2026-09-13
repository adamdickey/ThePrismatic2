using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class Subroutine2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/subroutine_power.png-2fcc05dbe94d52ea68296e1690b6f6af.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/subroutine_power.png-2fcc05dbe94d52ea68296e1690b6f6af.s3tc.ctex";
    
    private class Data
    {
        /// <summary>
        /// Keep track of the Power cards we've seen played and the power amount at the time they were played.
        /// This lets SubroutinePower avoid triggering on cards that started play before it was applied, and avoid
        /// granting extra energy on multiple plays of Subroutine.
        /// </summary>
        public readonly Dictionary<CardModel, int> AmountsForPlayedCards = new();
    }

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override object InitInternalData()
    {
        return new Data();
    }

    public override Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner.Player)
        {
            return Task.CompletedTask;
        }
        if (cardPlay.Card.Type != CardType.Power)
        {
            return Task.CompletedTask;
        }
        GetInternalData<Data>().AmountsForPlayedCards.Add(cardPlay.Card, Amount);
        return Task.CompletedTask;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner == Owner.Player && GetInternalData<Data>().AmountsForPlayedCards.Remove(cardPlay.Card, out var energy) && energy > 0)
        {
            Flash();
            for (int i = 0; i < energy; i++)
            {
                await PlayerCmd.GainEnergy(1m, Owner.Player);
                await PlayerCmd.GainStars(1m, Owner.Player);
            }
        }
    }
}
