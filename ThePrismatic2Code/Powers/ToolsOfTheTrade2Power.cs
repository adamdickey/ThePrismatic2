using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class ToolsOfTheTrade2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/tools_of_the_trade_power.png-6ee920e4139eb8e81b0dbf07230e1b88.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/tools_of_the_trade_power.png-6ee920e4139eb8e81b0dbf07230e1b88.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    private class Data
    {
        public readonly Dictionary<CardModel, int> PlayedCards = new();
    }
    
    protected override object InitInternalData()
    {
        return new Data();
    }

    public override decimal ModifyHandDraw(Player player, decimal count)
    {
        if (player != Owner.Player)
        {
            return count;
        }
        return count + Amount;
    }

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player == Owner.Player)
        {
            List<CardModel> list = (await CardSelectCmd.FromHandForDiscard(choiceContext, player, new CardSelectorPrefs(CardSelectorPrefs.DiscardSelectionPrompt, Amount), null, this)).ToList();
            if (list.Count != 0)
            {
                await CardCmd.Discard(choiceContext, list);
            }
        }
    }
    public override Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature == Owner && CombatState.CurrentSide == Owner.Side)
        {
            if (cardPlay.Card.Type == CardType.Power)
            {
                GetInternalData<Data>().PlayedCards.Add(cardPlay.Card, 0);
            }
        }
        return Task.CompletedTask;
    }
    
    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature == Owner && GetInternalData<Data>().PlayedCards.Remove(cardPlay.Card, out var _))
        {
            Flash();
            await CardPileCmd.Draw(context, Amount, cardPlay.Card.Owner);
            List<CardModel> list = (await CardSelectCmd.FromHandForDiscard(context, cardPlay.Card.Owner, new CardSelectorPrefs(CardSelectorPrefs.DiscardSelectionPrompt, Amount), null, this)).ToList();
            if (list.Count != 0)
            {
                await CardCmd.Discard(context, list);
            }
        }
    }
}
