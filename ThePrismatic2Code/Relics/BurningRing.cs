using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;

namespace ThePrismatic2.ThePrismatic2Code.Relics;

public sealed class BurningRing() : ThePrismatic2Relic
{
    public override RelicRarity Rarity =>
        RelicRarity.Starter;


    public override bool TryModifyCardRewardOptions(Player player, List<CardCreationResult> options, CardCreationOptions creationOptions)
    {
        if (base.Owner != player)
        {
            return false;
        }
        if (creationOptions.Source != CardCreationSource.Encounter)
        {
            return false;
        }

        
        
        IEnumerable<CardModel> enumerable = from c in creationOptions.GetPossibleCards(player)
            where options.TrueForAll((CardCreationResult o) => o.originalCard.Id != c.Id)
            select c;
        if (!enumerable.Any())
        {
            enumerable = from c in creationOptions.GetPossibleCards(player)
                select c;
        }
        if (!enumerable.Any())
        {
            return false;
        }
        CardCreationOptions options2 = new CardCreationOptions(enumerable, CardCreationSource.Other, creationOptions.RarityOdds).WithFlags(CardCreationFlags.NoModifyHooks | CardCreationFlags.NoCardPoolModifications);
        CardModel cardModel = CardFactory.CreateForReward(base.Owner, 1, options2).FirstOrDefault()?.Card;
        if (cardModel != null)
        {
            CardCreationResult cardCreationResult = new CardCreationResult(cardModel);
            cardCreationResult.ModifyCard(cardModel, this);
            options.Add(cardCreationResult);
        }
        return cardModel != null;
    }
}