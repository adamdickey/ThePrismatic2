using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class HiddenDaggers2() : ThePrismatic2Card(0, 
    CardType.Skill, CardRarity.Uncommon, 
    TargetType.Self)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<SilentCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/hidden_daggers.png-392999d6aa67de467931c43373a5c371.ctex";
    public override string PortraitPath => "res://.godot/imported/hidden_daggers.png-392999d6aa67de467931c43373a5c371.ctex";

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new CardsVar(2),
        new DynamicVar("Shivs", 2m)
    ]);

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<Shiv>(IsUpgraded));

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CardCmd.Discard(choiceContext, await CardSelectCmd.FromHandForDiscard(prefs: new CardSelectorPrefs(CardSelectorPrefs.DiscardSelectionPrompt, DynamicVars.Cards.IntValue), context: choiceContext, player: Owner, filter: null, source: this));
        if (CombatState != null)
        {
            IEnumerable<CardModel> enumerable = await Shiv.CreateInHand(Owner, DynamicVars["Shivs"].IntValue, CombatState);
            if (!IsUpgraded)
            {
                return;
            }
            foreach (CardModel item in enumerable)
            {
                CardCmd.Upgrade(item);
            }
        }
    }
}