using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Prepared2() : ThePrismatic2Card(0, 
    CardType.Skill, CardRarity.Common, 
    TargetType.Self)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<SilentCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/prepared.png-fd95e08bd21b946e2c3f636677bc1127.ctex";
    public override string PortraitPath => "res://.godot/imported/prepared.png-fd95e08bd21b946e2c3f636677bc1127.ctex";

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new CardsVar(1),
        new StarsVar(1)
        ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int cardCount = DynamicVars.Cards.IntValue;
        await CardPileCmd.Draw(choiceContext, cardCount, Owner);
        IEnumerable<CardModel> cards = await CardSelectCmd.FromHandForDiscard(choiceContext, Owner, new CardSelectorPrefs(CardSelectorPrefs.DiscardSelectionPrompt, cardCount), null, this);
        await CardCmd.Discard(choiceContext, cards);
        if (cards.Any(card => card.CanonicalStarCost > 0))
        {
            await PlayerCmd.GainStars(DynamicVars.Stars.BaseValue, Owner);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1m);
    }
}