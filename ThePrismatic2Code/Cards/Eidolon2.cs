using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Eidolon2() : ThePrismatic2Card(2, 
    CardType.Skill, CardRarity.Rare, 
    TargetType.Self)
{
	public override CardPoolModel VisualCardPool => ModelDb.CardPool<NecrobinderCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/eidolon.png-fab3ce75c381ad1515a4377e891db72c.ctex";
    public override string PortraitPath => "res://.godot/imported/eidolon.png-fab3ce75c381ad1515a4377e891db72c.ctex";

    public override bool CanBeGeneratedInCombat => false;

    public override IEnumerable<CardKeyword> CanonicalKeywords => new _003C_003Ez__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
	    HoverTipFactory.FromKeyword(CardKeyword.Ethereal),
	    HoverTipFactory.FromKeyword(Extensions.Keywords.Costly)
    ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
    	await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
	    IEnumerable<CardModel> enumerable = PileType.Exhaust.GetPile(Owner).Cards.Where(c => c.Id.Entry != "THEPRISMATIC2-EIDOLON2" || c.EnergyCost.GetWithModifiers(CostModifiers.All) + c.CurrentStarCost >= 2).ToList();
	    foreach (CardModel item in enumerable)
	    {
		    await CardCmd.AutoPlay(choiceContext, item, null);
	    }
    }

    protected override void OnUpgrade()
    {
    	EnergyCost.UpgradeBy(-1);
    }
}