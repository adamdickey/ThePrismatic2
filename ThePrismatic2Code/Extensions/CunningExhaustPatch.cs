using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace ThePrismatic2.ThePrismatic2Code.Extensions;

[HarmonyPatch(typeof(CardCmd), "Exhaust")]
public static class CunningExhaustPatch
{
    private static async void Postfix(PlayerChoiceContext choiceContext, CardModel card)
    {
        if (CombatManager.Instance.IsOverOrEnding)
        {
            return;
        }
        if (!card.Keywords.Contains(Keywords.Cunning))
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
        await CardCmd.AutoPlay(choiceContext, card, null, AutoPlayType.SlyDiscard);
        await CardPileCmd.Add(card, PileType.Exhaust);
    }
}