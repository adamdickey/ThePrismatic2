using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class ColdSnap2() : ThePrismatic2Card(1, 
    CardType.Attack, CardRarity.Common, 
    TargetType.AnyEnemy)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<DefectCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/cold_snap.png-dae83e0c99ed64415701baf52faef36b.ctex";
    public override string PortraitPath => "res://.godot/imported/cold_snap.png-dae83e0c99ed64415701baf52faef36b.ctex";
    
    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
	    new HpLossVar(1m),
	    new DamageVar(8m, ValueProp.Move) 
		]);
    
    	protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([ 
		    HoverTipFactory.FromKeyword(Extensions.Keywords.Bleed),
		    HoverTipFactory.Static(StaticHoverTip.Channeling),
    		HoverTipFactory.FromOrb<FrostOrb>()
	    ]);
    
    	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    	{
    		ArgumentNullException.ThrowIfNull(cardPlay.Target);
		    if (!Osty.CheckMissingWithAnim(Owner) && Owner.Osty != null)
		    {
			    VfxCmd.PlayOnCreatureCenter(Owner.Osty, "vfx/vfx_bloody_impact");
			    await CreatureCmd.Damage(choiceContext, Owner.Osty, DynamicVars.HpLoss.BaseValue, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this);
		    }
		    else
		    {
			    VfxCmd.PlayOnCreatureCenter(Owner.Creature, "vfx/vfx_bloody_impact");
			    await CreatureCmd.Damage(choiceContext, Owner.Creature, DynamicVars.HpLoss.BaseValue, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this);
		    }
    		await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
    			.WithHitFx("vfx/vfx_attack_slash")
    			.Execute(choiceContext);
    		await OrbCmd.Channel<FrostOrb>(choiceContext, Owner);
    	}
    
    	protected override void OnUpgrade()
    	{
    		DynamicVars.Damage.UpgradeValueBy(3m); 
		}
}