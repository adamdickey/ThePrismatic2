using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace ThePrismatic2.ThePrismatic2Code.Extensions;

[HarmonyPatch(typeof(CardCmd), "DiscardAndDraw")]
public static class CunningDiscardPatch
{
    private static async void Postfix(PlayerChoiceContext choiceContext, IEnumerable<CardModel> cardsToDiscard, int cardsToDraw)
    {
        List<CardModel> discardCards = cardsToDiscard.ToList();
        List<CardModel> cunningCards = discardCards.Where(card => card.Keywords.Contains(Keywords.Cunning)).ToList();
        foreach (CardModel item in cunningCards)
        {
            await CardCmd.AutoPlay(choiceContext, item, null, AutoPlayType.SlyDiscard);
        }
    }
}