using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.TestSupport;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class BouncingFlask2() : ThePrismatic2Card(2, 
    CardType.Skill, CardRarity.Uncommon, 
    TargetType.RandomEnemy)
{
	public override CardPoolModel VisualCardPool => ModelDb.CardPool<SilentCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/bouncing_flask.png-54c2b34c19221c30edf4900ff58824e2.ctex";
    public override string PortraitPath => "res://.godot/imported/bouncing_flask.png-54c2b34c19221c30edf4900ff58824e2.ctex";

    private readonly Color _vfxTint = new Color("83eb85");

	protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
		new PowerVar<PoisonPower>(2m),
		new PowerVar<DoomPower>(2m),
		new RepeatVar(3)
	]);

	protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
		HoverTipFactory.FromPower<PoisonPower>(),
		HoverTipFactory.FromPower<DoomPower>()
	]);

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
		Vector2 lastPos = Vector2.Zero;
		for (int i = 0; i < DynamicVars.Repeat.IntValue; i++)
		{
			if (CombatState != null)
			{
				Creature? enemy = Owner.RunState.Rng.CombatTargets.NextItem(CombatState.HittableEnemies);
				if (enemy == null)
				{
					continue;
				}
				if (TestMode.IsOff)
				{
					if (i == 0)
					{
						if (NCombatRoom.Instance != null)
							lastPos = NCombatRoom.Instance.GetCreatureNode(Owner.Creature)!.VfxSpawnPosition;
					}
					NCreature? targetNode = NCombatRoom.Instance?.GetCreatureNode(enemy);
					if (targetNode != null)
					{
						NItemThrowVfx? child = NItemThrowVfx.Create(lastPos, targetNode.GetBottomOfHitbox(), ModelDb.Potion<PoisonPotion>().Image);
						NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(child);
						lastPos = targetNode.VfxSpawnPosition;
						await Cmd.Wait(0.5f);
						NSplashVfx? child2 = NSplashVfx.Create(targetNode.VfxSpawnPosition, _vfxTint);
						NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(child2);
						NLiquidOverlayVfx? child3 = NLiquidOverlayVfx.Create(enemy, _vfxTint);
						NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(child3);
						NGaseousImpactVfx? child4 = NGaseousImpactVfx.Create(targetNode.VfxSpawnPosition, _vfxTint);
						NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(child4);
					}
				}
				await PowerCmd.Apply<PoisonPower>(choiceContext, enemy, DynamicVars.Poison.BaseValue, Owner.Creature, this);
				await PowerCmd.Apply<DoomPower>(choiceContext, enemy, DynamicVars.Doom.BaseValue, Owner.Creature, this);
			}
		}
	}

	protected override void OnUpgrade()
	{
		DynamicVars.Repeat.UpgradeValueBy(1m);
	}
}