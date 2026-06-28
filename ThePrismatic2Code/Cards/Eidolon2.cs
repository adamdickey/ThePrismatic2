using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
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
    
    	protected override bool ShouldGlowGoldInternal
    	{
    		get
    		{
    			PlayerCombatState? playerCombatState = Owner.PlayerCombatState;
    			if (playerCombatState == null)
    			{
    				return false;
    			}
    			return playerCombatState.Hand.Cards.Count > 9;
    		}
    	}
	    
	    public override IEnumerable<CardKeyword> CanonicalKeywords => new _003C_003Ez__ReadOnlySingleElementList<CardKeyword>(Extensions.Keywords.Cunning);
    
    	protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
		    HoverTipFactory.FromKeyword(CardKeyword.Exhaust),
    		HoverTipFactory.FromPower<IntangiblePower>()
	    ]);
    
    	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    	{
    		await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
		    if (Owner.PlayerCombatState != null)
		    {
			    List<CardModel> list = Owner.PlayerCombatState.Hand.Cards.ToList();
			    int exhaustedCount = 0;
			    foreach (CardModel item in list)
			    {
				    await CardCmd.Exhaust(choiceContext, item);
				    exhaustedCount++;
			    }
			    if (exhaustedCount >= 9)
			    {
				    await PowerCmd.Apply<IntangiblePower>(choiceContext, Owner.Creature, 1m, Owner.Creature, this);
			    }
		    }
	    }
    
    	protected override void OnUpgrade()
    	{
    		EnergyCost.UpgradeBy(-1);
    	}
}