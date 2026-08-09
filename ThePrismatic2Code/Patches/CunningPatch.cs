using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using ThePrismatic2.ThePrismatic2Code.Extensions;

namespace ThePrismatic2.ThePrismatic2Code.Patches;

[HarmonyPatch(typeof(CardCmd), "Exhaust")]
public static class CunningPatch
{
    private static void Postfix(PlayerChoiceContext choiceContext, CardModel card)
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
            if (CombatManager.Instance.History.Entries.OfType<CardPlayFinishedEntry>().LastOrDefault()?.CardPlay.Card == card)
            {
                return;
            }
        }
        CardCmd.AutoPlay(choiceContext, card, null, AutoPlayType.SlyDiscard);
    }
}


[HarmonyPatch(typeof(CardCmd), "DiscardAndDraw")]
public static class CunningDiscardPatch
{
    private static void Postfix(PlayerChoiceContext choiceContext, IEnumerable<CardModel> cardsToDiscard, int cardsToDraw)
    {
        List<CardModel> cunningCards = cardsToDiscard.Where(card => card.Keywords.Contains(Keywords.CunningThisTurn)).ToList();
        foreach (CardModel item in cunningCards)
        {
            CardCmd.AutoPlay(choiceContext, item, null, AutoPlayType.SlyDiscard);
        }
    }
}