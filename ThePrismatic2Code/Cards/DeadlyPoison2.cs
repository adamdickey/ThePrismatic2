using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class DeadlyPoison2() : ThePrismatic2Card(1, 
    CardType.Skill, CardRarity.Common, 
    TargetType.AnyEnemy)
{
	public override CardPoolModel VisualCardPool => ModelDb.CardPool<SilentCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/deadly_poison.png-a6ee1c7032689fc18ee81a555c0fe6f1.ctex";
    public override string PortraitPath => "res://.godot/imported/deadly_poison.png-a6ee1c7032689fc18ee81a555c0fe6f1.ctex";

    protected override IEnumerable<string> ExtraRunAssetPaths => NSmokePuffVfx.AssetPaths;
    
    	protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
		    new PowerVar<PoisonPower>(4m),
		    new PowerVar<DoomPower>(4m)
		    ]);
    
    	protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
		    HoverTipFactory.FromPower<PoisonPower>(),
		    HoverTipFactory.FromPower<DoomPower>()
	    ]);
    
    	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    	{
    		ArgumentNullException.ThrowIfNull(cardPlay.Target);
    		NPoisonImpactVfx? child = NPoisonImpactVfx.Create(cardPlay.Target);
    		NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(child);
    		await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
    		await PowerCmd.Apply<PoisonPower>(choiceContext, cardPlay.Target, DynamicVars.Poison.BaseValue, Owner.Creature, this);
			await PowerCmd.Apply<DoomPower>(choiceContext, cardPlay.Target, DynamicVars.Doom.BaseValue,  Owner.Creature, this);
    	}
    
    	protected override void OnUpgrade()
    	{
    		DynamicVars.Poison.UpgradeValueBy(2m);
		    DynamicVars.Doom.UpgradeValueBy(2m);
    	}
}