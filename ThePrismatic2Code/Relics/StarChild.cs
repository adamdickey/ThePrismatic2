using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using ThePrismatic2.ThePrismatic2Code.Enchantments;

namespace ThePrismatic2.ThePrismatic2Code.Relics;

public sealed class StarChild: ThePrismatic2Relic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    public override bool HasUponPickupEffect => true;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        ..HoverTipFactory.FromEnchantment<Starbound>()
    ]);

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new StarsVar(1),
        new CardsVar(3)
    ]);

    public override async Task AfterObtained()
    {
        List<CardModel> deck = PileType.Deck.GetPile(Owner).Cards.Where(c => c.Enchantment == null).ToList();
        if (deck.Count == 0)
        {
            return;
        }
        List<CardModel> chosen = (await CardSelectCmd.FromSimpleGrid(
            new BlockingPlayerChoiceContext(),
            deck,
            Owner,
            new CardSelectorPrefs(SelectionScreenPrompt, DynamicVars.Cards.IntValue))).ToList();
        if (chosen.Count == 0)
        {
            return;
        }
        foreach (CardModel card in chosen)
        {
            CardCmd.Enchant<Starbound>(card, 1m);
            CardCmd.Preview(card, 1f);
            await Cmd.Wait(0.01f);
        }
        await Cmd.Wait(0.75f);
    }

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player == Owner)
        {
            Flash();
            await PlayerCmd.GainStars(DynamicVars.Stars.BaseValue, Owner);
        }
    }
}
