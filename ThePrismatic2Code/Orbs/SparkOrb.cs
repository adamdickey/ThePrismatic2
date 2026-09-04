using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.ValueProps;

namespace ThePrismatic2.ThePrismatic2Code.Orbs;


public sealed class SparkOrb : CustomOrbModel
{
    public override Color DarkenedColor => new("796606");
    public override string CustomIconPath => "res://.godot/imported/lightning_orb.png-c2e70ef3c469cfc84adbcdcd11711dad.ctex";
    public override bool IncludeInRandomPool => false;
    
    public override string CustomPassiveSfx => "event:/sfx/characters/defect/defect_lightning_passive";
    public override string CustomEvokeSfx => "event:/sfx/characters/defect/defect_lightning_evoke";
    public override string CustomChannelSfx => "event:/sfx/characters/defect/defect_lightning_channel";

    public override decimal PassiveVal => ModifyOrbValue(2m);
    public override decimal EvokeVal => ModifyOrbValue(5m);

    public override Node2D CreateCustomSprite()
    {
        var container = new Node2D();
        string lightningPath = SceneHelper.GetScenePath("orbs/orb_visuals/lightning_orb");
        Node2D lightning = PreloadManager.Cache.GetScene(lightningPath)
            .Instantiate<Node2D>();
        new MegaSprite(lightning.GetNode("SpineSkeleton"))
            .GetAnimationState().SetAnimation("idle_loop");
        lightning.Scale = new Vector2(0.9f, 0.9f);
        container.AddChild(lightning);
        return container;
    }
    
    public override async Task BeforeTurnEndOrbTrigger(PlayerChoiceContext choiceContext)
    {
        await Passive(choiceContext, null);
    }

    public override async Task Passive(PlayerChoiceContext choiceContext, Creature? target)
    {
        Trigger();
        await ApplyLightningDamage(PassiveVal, target, choiceContext);
    }

    public override async Task<IEnumerable<Creature>> Evoke(PlayerChoiceContext playerChoiceContext)
    {
        return await ApplyLightningDamage(EvokeVal, null, playerChoiceContext);
    }

    private async Task<IEnumerable<Creature>> ApplyLightningDamage(decimal value, Creature? target, PlayerChoiceContext choiceContext)
    {
        List<Creature> list = (from e in CombatState.GetOpponentsOf(Owner.Creature)
            where e.IsHittable
            select e).ToList();
        if (list.Count == 0)
        {
            return Array.Empty<Creature>();
        }
        IReadOnlyList<Creature> targets = target == null ? new _003C_003Ez__ReadOnlySingleElementList<Creature>(Owner.RunState.Rng.CombatTargets.NextItem(list) ?? throw new InvalidOperationException()) : new _003C_003Ez__ReadOnlySingleElementList<Creature>(target);
        foreach (Creature item in targets)
        {
            VfxCmd.PlayOnCreature(item, "vfx/vfx_attack_lightning");
        }
        PlayEvokeSfx();
        await CreatureCmd.Damage(choiceContext, targets, value, ValueProp.Unpowered, Owner.Creature);
        return targets;
    }
}
