using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;
using ThePrismatic2.ThePrismatic2Code.Cards;
using ThePrismatic2.ThePrismatic2Code.Extensions;

namespace ThePrismatic2.ThePrismatic2Code.Relics;

public sealed class BurningRing: ThePrismatic2Relic
{
    public override RelicRarity Rarity =>
        RelicRarity.Starter;

    private int[] _colorInts = [];

    [SavedProperty]
    public int[] ColorInts
    {
        get => _colorInts;
        private set
        {
            AssertMutable();
            _colorInts = value;
        }
    }

    // Adds the Delete option to rest sites.
    public override bool TryModifyRestSiteOptions(Player player, ICollection<RestSiteOption> options)
    {
        if (player != Owner)
        {
            return false;
        }
        options.Add(new DeleteRestSiteOption(player));
        return true;
    }
    
    // Generates card reward options excluding any removed colors.
    public override CardCreationOptions ModifyCardRewardCreationOptions(Player player, CardCreationOptions options)
    {
        if (player != Owner)
        {
            return options;
        }
        CardModel[] colorsRemoved = ColorsRemoved();
        List<CardPoolModel> colors = colorsRemoved.Select(card => card.VisualCardPool).ToList();
        IEnumerable<CardModel> enumerable = from c in options.GetPossibleCards(player)
            where !colors.Contains(c.VisualCardPool) select c;
        CardCreationOptions options2 = new CardCreationOptions(enumerable, options.Source, options.RarityOdds);
        return options2;
    }

    // Adds a color to the list of colors to exclude.
    public void RemoveColor(int index)
    {
        ColorInts = ColorInts.Append(index).ToArray();
    }

    // Returns a list of colors removed from the pool.
    public CardModel[] ColorsRemoved()
    {
        CardModel[] colorsRemoved = [];
        foreach (int num in ColorInts)
        {
            CardModel card = ModelDb.Card<Red>();
            switch (num)
            {
                case 0:
                    card = ModelDb.Card<Red>();
                    break;
                case 1:
                    card = ModelDb.Card<Green>();
                    break;
                case 2:
                    card = ModelDb.Card<Orange>();
                    break;
                case 3:
                    card = ModelDb.Card<Pink>();
                    break;
                case 4:
                    card = ModelDb.Card<Blue>();
                    break;
            }
            colorsRemoved = colorsRemoved.Append(card).ToArray();
        }
        return colorsRemoved;
    }
}