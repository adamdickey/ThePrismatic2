using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using ThePrismatic2.ThePrismatic2Code.Cards;
using ThePrismatic2.ThePrismatic2Code.Relics;

namespace ThePrismatic2.ThePrismatic2Code.Extensions;

public class DeleteRestSiteOption(Player owner) : RestSiteOption(owner)
{
    
    public override IEnumerable<string> AssetPaths => new _003C_003Ez__ReadOnlySingleElementList<string>("res://.godot/imported/option_toke.png-9ecec93d7e032c53a13cf05303343738.ctex");
    public override string OptionId => "TOKE";
    
    public override bool IsEnabled
    {
        get
        {
            BurningRing? burningRing = Owner.GetRelic<BurningRing>();
            if (burningRing != null)
            {
                return burningRing.ColorsRemoved().Length < 2;
            }
            return false;
        }
    }
    
    public override LocString Description
    	{
    		get
            {
                LocString description = IsEnabled ? base.Description : new LocString("rest_site_ui", "OPTION_" + OptionId + ".descriptionDisabled");
                BurningRing? burningRing = Owner.GetRelic<BurningRing>();
                string colorString = "";
                if (burningRing != null)
                {
                    if (burningRing.ColorInts.Length == 0)
                    {
                        description.Add("Colors", colorString);
                        return description;
                    }
                    colorString += "\n(";
                    for (int i = 0; i < burningRing.ColorInts.Count(); i++)
                    {
                        int num = burningRing.ColorInts[i];
                        string colorName = num switch
                        {
                            0 => "[red]Red[/red]",
                            1 => "[green]Green[/green]",
                            2 => "[orange]Orange[/orange]",
                            3 => "[pink]Pink[/pink]",
                            4 => "[blue]Blue[/blue]",
                            _ => ""
                        };
                        if (num != burningRing.ColorInts[^1] && burningRing.ColorInts.Length >= 3)
                        {
                            colorString += colorName + ", ";
                        }
                        else if (num == burningRing.ColorInts[^1] && burningRing.ColorInts.Length >= 2)
                        {
                            colorString += "and " + colorName;
                        }
                        else if (num != burningRing.ColorInts[^1])
                        {
                            colorString += colorName + " ";
                        }
                        else
                        {
                            colorString += colorName;
                        }
                    }
                    colorString += " previously removed.)";
                    description.Add("Colors", colorString);
                }
                return description;
            }
    	}

    public override async Task<bool> OnSelect()
    {
        BurningRing? burningRing = Owner.GetRelic<BurningRing>();
        CardModel[] colorsRemoved = [];
        if (burningRing != null)
        {
            colorsRemoved = burningRing.ColorsRemoved();
        }
        CardModel[] allColors =
        [
            ModelDb.Card<Red>(), ModelDb.Card<Green>(), ModelDb.Card<Orange>(), ModelDb.Card<Pink>(),
            ModelDb.Card<Blue>()
        ];
        CardModel[] colorsLeft = allColors.Where(card => !colorsRemoved.Contains(card)).ToArray();
        Random.Shared.Shuffle(colorsLeft);
        IReadOnlyList<CardModel> cardOptions = colorsLeft.Take(3).ToList();
        
        CardModel cardModel = await CardSelectCmd.FromChooseACardScreen(new BlockingPlayerChoiceContext(), cardOptions, Owner) ?? throw new InvalidOperationException();
        int cardIndex;
        if (cardModel == ModelDb.Card<Red>()) cardIndex = 0;
        else if (cardModel == ModelDb.Card<Green>()) cardIndex = 1;
        else if (cardModel == ModelDb.Card<Orange>()) cardIndex = 2;
        else if (cardModel == ModelDb.Card<Pink>()) cardIndex = 3;
        else if (cardModel == ModelDb.Card<Blue>()) cardIndex = 4;
        else return false;
        burningRing?.RemoveColor(cardIndex);
        return true;
    }
}