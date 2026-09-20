using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace ThePrismatic2.ThePrismatic2Code.Relics;

public sealed class Kaleidoscope2: ThePrismatic2Relic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    public override string PackedIconPath => "res://.godot/imported/kaleidoscope_small.png-b7ec5951675b1bfefdbf98cd1108249e.ctex";
    protected override string PackedIconOutlinePath => "res://.godot/imported/kaleidoscope_small_stroke.png-3ddbbc3613129f5343dbda96643787c6.ctex";
    protected override string BigIconPath => "res://.godot/imported/kaleidoscope.png-50183c1992451ff0384dfd85916a6fe2.ctex";

    public override bool HasUponPickupEffect => true;

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new CardsVar(1));

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Transform));

    public override async Task AfterObtained()
    {
        List<CardModel> source = PileType.Deck.GetPile(Owner).Cards.Where(c => c.Rarity == CardRarity.Basic).ToList();
        CardModel? cardModel = source.FirstOrDefault(c => c.Tags.Contains(CardTag.Strike));
        CardModel? cardModel2 = source.FirstOrDefault(c => c.Tags.Contains(CardTag.Defend));
        List<CardTransformation> list = new List<CardTransformation>();
        string? strikeName = Owner.Deck.Cards.FirstOrDefault(c => c.Rarity != CardRarity.Basic && c.Tags.Contains(CardTag.Strike))?.Id.ToString();
        string? strikeType = strikeName ?? strikeName?.Replace("Strike", "");
        string? defendName = Owner.Deck.Cards.FirstOrDefault(c => c.Rarity != CardRarity.Basic && c.Tags.Contains(CardTag.Defend))?.Id.ToString();
        string? defendType = defendName ?? defendName?.Replace("Defend", "");
        if (cardModel != null && cardModel2 != null)
        {
            CardModel strike = Character.ThePrismatic2.GetPrismaticStrikes().TakeRandom(1, Owner.RunState.Rng.Niche).First();
            CardModel defend = Character.ThePrismatic2.GetPrismaticDefends().TakeRandom(1, Owner.RunState.Rng.Niche).First();
            if (strikeType != null && defendType != null)
            {
                while (true)
                {
                    strike = Character.ThePrismatic2.GetPrismaticStrikes().TakeRandom(1, Owner.RunState.Rng.Niche).First();
                    defend = Character.ThePrismatic2.GetPrismaticDefends().TakeRandom(1, Owner.RunState.Rng.Niche).First();
                    if (strike.Id.ToString().Contains(strikeType) || strike.Id.ToString().Contains(defendType))
                    {
                        continue;
                    }
                    if (defend.Id.ToString().Contains(strikeType) || defend.Id.ToString().Contains(defendType))
                    {
                        continue;
                    }
                    break;
                }
            }
            CardModel newCard = Owner.RunState.CreateCard(strike, Owner);
            CardModel newCard2 = Owner.RunState.CreateCard(defend, Owner);
        	list.Add(new CardTransformation(cardModel, newCard));
            list.Add(new CardTransformation(cardModel2, newCard2));
        }
        await CardCmd.Transform(list, Owner.PlayerRng.Transformations);
    }
}