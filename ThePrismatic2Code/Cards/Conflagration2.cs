using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Conflagration2() : ThePrismatic2Card(1, 
    CardType.Attack, CardRarity.Rare, 
    TargetType.AllEnemies)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<IroncladCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/conflagration.png-e304f5493cd41c4a3f93cdf71cbf8d4e.ctex";
    public override string PortraitPath => "res://.godot/imported/conflagration.png-e304f5493cd41c4a3f93cdf71cbf8d4e.ctex";
    
    protected override IEnumerable<string> ExtraRunAssetPaths => NGroundFireVfx.AssetPaths;
    
    	protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
    		new DamageVar(2m, ValueProp.Move),
    		new RepeatVar(4),
			new StarsVar(1)
    	]);
    
    	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    	{
		    if (CombatState != null)
		    {
			    IReadOnlyList<Creature> hittableEnemies = CombatState.HittableEnemies;
			    foreach (Creature item in hittableEnemies)
			    {
				    NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(NGroundFireVfx.Create(item));
			    }
			    await DamageCmd.Attack(DynamicVars.Damage.BaseValue).WithHitCount(DynamicVars.Repeat.IntValue).FromCard(this)
				    .TargetingAllOpponents(CombatState)
				    .WithHitFx("vfx/vfx_attack_blunt", null, "heavy_attack.mp3")
				    .Execute(choiceContext);
		    }
			await PlayerCmd.GainStars(DynamicVars.Stars.BaseValue, Owner);
    	}
    
    	protected override void OnUpgrade()
    	{
    		DynamicVars.Repeat.UpgradeValueBy(1m);
			DynamicVars.Stars.UpgradeValueBy(1m);
    	}
}