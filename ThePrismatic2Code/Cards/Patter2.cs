using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;
using ThePrismatic2.ThePrismatic2Code.Orbs;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Patter2() : ThePrismatic2Card(1, 
    CardType.Skill, CardRarity.Common, 
    TargetType.Self)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<RegentCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/patter.png-8c463eda35af5d841d726cb685fbaf6a.ctex";
    public override string PortraitPath => "res://.godot/imported/patter.png-8c463eda35af5d841d726cb685fbaf6a.ctex";
    
    public override bool GainsBlock => true;
    
	protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new BlockVar(8m, ValueProp.Move));
    
	protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
		HoverTipFactory.Static(StaticHoverTip.Channeling),
		HoverTipFactory.FromOrb<MagmaOrb>()
	]);
    
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
		await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
		await OrbCmd.Channel<MagmaOrb>(choiceContext, Owner);
	}
    
	protected override void OnUpgrade()
	{
		DynamicVars.Block.UpgradeValueBy(3m);
	}
}