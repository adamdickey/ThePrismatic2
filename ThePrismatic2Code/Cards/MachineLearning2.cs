using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using ThePrismatic2.ThePrismatic2Code.Character;
using ThePrismatic2.ThePrismatic2Code.Powers;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class MachineLearning2() : ThePrismatic2Card(1, 
    CardType.Power, CardRarity.Rare, 
    TargetType.Self)
{
	public override CardPoolModel VisualCardPool => ModelDb.CardPool<DefectCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/machine_learning.png-ea3f3ddf149bf8eaf9c319dc60ce57d9.ctex";
    public override string PortraitPath => "res://.godot/imported/machine_learning.png-ea3f3ddf149bf8eaf9c319dc60ce57d9.ctex";
    
    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new CardsVar(1));
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<Soul>());
    
    	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    	{
    		await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
    		await PowerCmd.Apply<MachineLearning2Power>(choiceContext, Owner.Creature, DynamicVars.Cards.BaseValue, Owner.Creature, this);
    	}
    
    	protected override void OnUpgrade()
    	{
    		AddKeyword(CardKeyword.Innate);
    	}
}