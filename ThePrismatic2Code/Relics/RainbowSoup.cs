using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Screens;

namespace ThePrismatic2.ThePrismatic2Code.Relics;

public sealed class RainbowSoup: ThePrismatic2Relic
{
    public override string PackedIconPath => "res://images/atlases/relic_atlas.sprites/nutritious_soup.tres";
    protected override string PackedIconOutlinePath => "res://images/atlases/relic_outline_atlas.sprites/nutritious_soup.tres";
    protected override string BigIconPath => "res://images/relics/nutritious_soup.png";
    
    public override RelicRarity Rarity => RelicRarity.Ancient;

    public override bool HasUponPickupEffect => true;

    public override async Task AfterObtained()
    {
        List<CardModel> source = PileType.Deck.GetPile(Owner).Cards.Where(c => c.IsBasicStrikeOrDefend && c.IsRemovable).ToList();
        IEnumerable<CardTransformation> transformations = source.Select(c => new CardTransformation(c, c.Tags.Contains(CardTag.Strike) ? Character.ThePrismatic2.GetRandomPrismaticStrike() : Character.ThePrismatic2.GetRandomPrismaticDefend()));
        List<CardPileAddResult> list = (await CardCmd.Transform(transformations, null, CardPreviewStyle.None)).ToList();
        if (list.Count > 0 && LocalContext.IsMe(Owner))
        {
            NSimpleCardsViewScreen.ShowScreen(list, new LocString("relics", "THEPRISMATIC2-RAINBOW_SOUP.infoText"));
        }
    }

    
}