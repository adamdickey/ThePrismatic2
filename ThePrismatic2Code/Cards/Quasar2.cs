using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Quasar2() : ThePrismatic2Card(0, 
    CardType.Skill, CardRarity.Uncommon, 
    TargetType.Self)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<RegentCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/quasar.png-7bc37e81f4414a6a5e14f44900fff71d.ctex";
    public override string PortraitPath => "res://.godot/imported/quasar.png-7bc37e81f4414a6a5e14f44900fff71d.ctex";

    public override int CanonicalStarCost => 2;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Exhaust));
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => new _003C_003Ez__ReadOnlySingleElementList<CardKeyword>(Extensions.Keywords.Starbound);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CardModel selection = (await CardSelectCmd.FromHand(prefs: new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, 1), context: choiceContext, player: Owner, filter: null, source: this)).FirstOrDefault() ?? throw new InvalidOperationException();
        await CardCmd.Exhaust(choiceContext, selection);
        List<CardModel> cards = CardFactory.GetDistinctForCombat(Owner, ModelDb.CardPool<ColorlessCardPool>().GetUnlockedCards(Owner.UnlockState, Owner.RunState.CardMultiplayerConstraint), 3, Owner.RunState.Rng.CombatCardGeneration).ToList();
        if (IsUpgraded)
        {
            CardCmd.Upgrade(cards, CardPreviewStyle.HorizontalLayout);
        }
        CardModel? cardModel = await CardSelectCmd.FromChooseACardScreen(choiceContext, cards, Owner, canSkip: true);
        if (cardModel != null)
        {
            await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Hand, Owner);
        }
    }
}