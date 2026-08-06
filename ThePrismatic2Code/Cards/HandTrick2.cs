using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class HandTrick2() : ThePrismatic2Card(1, 
    CardType.Skill, CardRarity.Uncommon, 
    TargetType.Self)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<SilentCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/hand_trick.png-1fbb03063f6f7e57282622e749015344.ctex";
    public override string PortraitPath => "res://.godot/imported/hand_trick.png-1fbb03063f6f7e57282622e749015344.ctex";
    
    public override bool GainsBlock => true;

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new BlockVar(6m, ValueProp.Move));

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        HoverTipFactory.FromKeyword(Extensions.Keywords.Cunning),
        HoverTipFactory.FromKeyword(Extensions.Keywords.Starbound),
        HoverTipFactory.FromKeyword(CardKeyword.Retain)
    ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        CardModel? cardModel = (await CardSelectCmd.FromHand(prefs: new CardSelectorPrefs(SelectionScreenPrompt, 1), context: choiceContext, player: Owner, filter: card => card.Type == CardType.Skill && (
            !(card.Keywords.Contains(Extensions.Keywords.Cunning) || !card.Keywords.Contains(Extensions.Keywords.CunningThisTurn)) ||
            !(card.Keywords.Contains(Extensions.Keywords.Starbound) || !card.Keywords.Contains(Extensions.Keywords.StarboundThisTurn)) ||
            !card.Keywords.Contains(CardKeyword.Retain)
        ), source: this)).FirstOrDefault();
        
        if (cardModel != null)
        {
            if (!cardModel.Keywords.Contains(Extensions.Keywords.Cunning) && !cardModel.Keywords.Contains(Extensions.Keywords.CunningThisTurn))
            {
                cardModel.AddKeyword(Extensions.Keywords.CunningThisTurn);
            }
            if (!cardModel.Keywords.Contains(Extensions.Keywords.Starbound) && !cardModel.Keywords.Contains(Extensions.Keywords.StarboundThisTurn))
            {
                cardModel.AddKeyword(Extensions.Keywords.StarboundThisTurn);
            }
            if (!cardModel.Keywords.Contains(CardKeyword.Retain))
            {
                cardModel.GiveSingleTurnRetain();
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3m);
    }
}