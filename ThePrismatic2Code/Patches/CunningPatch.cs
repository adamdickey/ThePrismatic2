using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using ThePrismatic2.ThePrismatic2Code.Extensions;

namespace ThePrismatic2.ThePrismatic2Code.Patches;

public class CunningSingleton() : CustomSingletonModel(true, false)
{
    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
    {
        if (CombatManager.Instance.IsOverOrEnding)
        {
            return;
        }
        if (!card.Keywords.Contains(Keywords.Cunning) && !card.Keywords.Contains(Keywords.CunningThisTurn))
        {
            return;
        }
        if (card.ExhaustOnNextPlay || card.Keywords.Contains(CardKeyword.Exhaust))
        {
            List<CardPlayStartedEntry> entries = CombatManager.Instance.History.Entries.OfType<CardPlayStartedEntry>().ToList();
            CardModel previousCard = entries[^2].CardPlay.Card;
            if (previousCard == card)
            {
                return;
            }
        }
        await CardCmd.AutoPlay(choiceContext, card, null, AutoPlayType.SlyDiscard);
    }
    
    public override async Task AfterCardDiscarded(PlayerChoiceContext choiceContext, CardModel card)
    {
        if (CombatManager.Instance.IsOverOrEnding)
        {
            return;
        }
        if (!card.Keywords.Contains(Keywords.Cunning) && !card.Keywords.Contains(Keywords.CunningThisTurn))
        {
            return;
        }
        await CardCmd.AutoPlay(choiceContext, card, null, AutoPlayType.SlyDiscard);
    }
}