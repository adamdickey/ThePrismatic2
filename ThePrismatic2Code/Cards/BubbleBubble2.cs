using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class BubbleBubble2() : ThePrismatic2Card(1, 
    CardType.Skill, CardRarity.Uncommon, 
    TargetType.AnyEnemy)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<SilentCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/bubble_bubble.png-125cb0b9a1c0e99d59d54aa37df57c4f.ctex";
    public override string PortraitPath => "res://.godot/imported/bubble_bubble.png-125cb0b9a1c0e99d59d54aa37df57c4f.ctex";
    protected override bool ShouldGlowGoldInternal => CombatState?.HittableEnemies.Any(e => e.Powers.Any(power => power is { Type: PowerType.Debuff, Applier.IsPlayer: true })) ?? false;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        HoverTipFactory.FromPower<PoisonPower>(),
        HoverTipFactory.FromPower<DoomPower>()
    ]);

    protected override IEnumerable<string> ExtraRunAssetPaths => NSmokePuffVfx.AssetPaths;

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new PowerVar<PoisonPower>(3m),
        new PowerVar<DoomPower>(3m),
        new CalculationBaseVar(0m),
        new CalculationExtraVar(1m),
        new CalculatedVar("CalculatedDebuffs").WithMultiplier((card, target) => target != null ? card.DynamicVars["PoisonPower"].IntValue*target.Powers.Count(power => power is { Type: PowerType.Debuff, Applier.IsPlayer: true }) : 0)
    ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        int debuffCount = cardPlay.Target.Powers.Count(power => power is { Type: PowerType.Debuff, Applier.IsPlayer: true });
        if (debuffCount >= 1)
        {
            await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
            NCreature? nCreature = NCombatRoom.Instance?.GetCreatureNode(cardPlay.Target);
            if (nCreature != null)
            {
                NGaseousImpactVfx? child = NGaseousImpactVfx.Create(nCreature.VfxSpawnPosition, new Color("83eb85"));
                NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(child);
            }
            await PowerCmd.Apply<PoisonPower>(choiceContext, cardPlay.Target, DynamicVars.Poison.BaseValue*debuffCount, Owner.Creature, this);
            await PowerCmd.Apply<DoomPower>(choiceContext, cardPlay.Target, DynamicVars.Doom.BaseValue*debuffCount, Owner.Creature, this);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Doom.UpgradeValueBy(1m);
        DynamicVars.Poison.UpgradeValueBy(1m);
    }
}