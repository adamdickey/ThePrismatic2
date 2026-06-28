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
public class DoubleEnergy2() : ThePrismatic2Card(1, 
    CardType.Skill, CardRarity.Uncommon, 
    TargetType.Self)
{
	public override CardPoolModel VisualCardPool => ModelDb.CardPool<DefectCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/double_energy.png-ae2a949d525c008ad2a7ed9bc1529495.ctex";
    public override string PortraitPath => "res://.godot/imported/double_energy.png-ae2a949d525c008ad2a7ed9bc1529495.ctex";
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => new _003C_003Ez__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
    
    	protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(EnergyHoverTip);
    
    	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	    {
		    if (Owner.PlayerCombatState != null)
		    {
			    await PlayerCmd.GainEnergy(Owner.PlayerCombatState.Energy, Owner);
			    await PlayerCmd.GainStars(Owner.PlayerCombatState.Stars, Owner);
		    }
	    }
    
    	protected override void OnUpgrade()
    	{
    		EnergyCost.UpgradeBy(-1);
    	}
}