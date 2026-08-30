using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Begone2() : ThePrismatic2Card(1, 
    CardType.Skill, CardRarity.Common, 
    TargetType.Self)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<RegentCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/begone.png-fe68822900425d35ccc3771a48570f9a.ctex";
    public override string PortraitPath => "res://.godot/imported/begone.png-fe68822900425d35ccc3771a48570f9a.ctex";

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        HoverTipFactory.FromCard<MinionStrike>(IsUpgraded),
        HoverTipFactory.FromKeyword(CardKeyword.Exhaust)
        ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CardModel? cardModel = (await CardSelectCmd.FromHand(prefs: new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, 1), context: choiceContext, player: Owner, filter: null, source: this)).FirstOrDefault();
        if (CombatState != null)
        {
            CardModel cardModel2 = CombatState.CreateCard<MinionStrike>(Owner);
            if (IsUpgraded)
            {
                CardCmd.Upgrade(cardModel2);
            }
            if (cardModel != null) await CardCmd.Exhaust(choiceContext, cardModel);
            await CardPileCmd.AddGeneratedCardToCombat(cardModel2, PileType.Hand, Owner);
        }
    }
}