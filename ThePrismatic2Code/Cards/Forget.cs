using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using ThePrismatic2.ThePrismatic2Code.Extensions;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

public class Forget() : ThePrismatic2Card(1, 
    CardType.Status, CardRarity.Status, 
    TargetType.None)
{
    public override string CustomPortraitPath => $"Forget.png".BigCardImagePath();
    public override string PortraitPath => $"Forget.png".CardImagePath();

    public override int MaxUpgradeLevel => 0;

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new CardsVar(1));

    public override IEnumerable<CardKeyword> CanonicalKeywords => new _003C_003Ez__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CardCmd.Discard(choiceContext, await CardSelectCmd.FromHandForDiscard(choiceContext, Owner, new CardSelectorPrefs(CardSelectorPrefs.DiscardSelectionPrompt, (int)DynamicVars.Cards.BaseValue), null, this));
    }
}