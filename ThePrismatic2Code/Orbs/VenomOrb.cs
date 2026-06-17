using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace ThePrismatic2.ThePrismatic2Code.Orbs;


public sealed class VenomOrb : CustomOrbModel
{
    public override Color DarkenedColor => new("2d6e2d");
    public override string CustomIconPath => "res://.godot/imported/dark_orb.png-5f06b12b25c362f6d903a63dbe2e565f.ctex";
    public override bool IncludeInRandomPool => true;

    // Reuse Dark Orb sounds - practical use of overrides
    public override string CustomPassiveSfx => "event:/sfx/characters/defect/defect_dark_passive";
    public override string CustomEvokeSfx => "event:/sfx/characters/defect/defect_dark_evoke";
    public override string CustomChannelSfx => "event:/sfx/characters/defect/defect_dark_channel";

    private decimal _passiveVal = 2m;

    public override decimal PassiveVal => ModifyOrbValue(_passiveVal);

    public override decimal EvokeVal => PassiveVal * 2m;

    public override Node2D CreateCustomSprite()
    {
        var container = new Node2D();
        string darkPath = SceneHelper.GetScenePath("orbs/orb_visuals/dark_orb");
        Node2D dark = PreloadManager.Cache.GetScene(darkPath)
            .Instantiate<Node2D>();
        new MegaSprite(dark.GetNode("SpineSkeleton"))
            .GetAnimationState().SetAnimation("idle_loop");
        dark.Modulate = _passiveVal <= 0m ? new Color(0.1f, 0.5f, 0.1f, 0.0f) : new Color(0.1f, 0.5f, 0.1f);
        container.AddChild(dark);
        string glassPath = SceneHelper.GetScenePath("orbs/orb_visuals/glass_orb");
        Node2D glass = PreloadManager.Cache.GetScene(glassPath)
            .Instantiate<Node2D>();
        new MegaSprite(glass.GetNode("SpineSkeleton"))
            .GetAnimationState().SetAnimation("idle_loop");
        glass.Modulate = _passiveVal <= 0m ? new Color(0.3f, 0.9f, 0.3f, 0.0f) : new Color(0.3f, 0.9f, 0.3f);
        glass.Scale = new Vector2(0.9f, 0.9f);
        container.AddChild(glass);
        return container;
    }

    public override async Task BeforeTurnEndOrbTrigger(PlayerChoiceContext choiceContext)
    {
        await Passive(choiceContext, null);
    }

    public override async Task Passive(PlayerChoiceContext choiceContext, Creature? target)
    {
        List<Creature> targets = CombatState.HittableEnemies.Where(e => e.IsHittable).ToList();
        decimal passiveVal = PassiveVal;
        if (!(passiveVal <= 0m))
        {
            Trigger();
            PlayPassiveSfx();
            await PowerCmd.Apply<PoisonPower>(targets, PassiveVal, Owner.Creature, null);
            _passiveVal = Math.Max(0m, _passiveVal - 1m);
        }
    }

    public override async Task<IEnumerable<Creature>> Evoke(PlayerChoiceContext playerChoiceContext)
    {
        List<Creature> enemies = CombatState.HittableEnemies.Where(e => e.IsHittable).ToList();
        if (EvokeVal <= 0m)
        {
            return Array.Empty<Creature>();
        }
        foreach (Creature hittableEnemy in CombatState.HittableEnemies)
        {
            NCreature? nCreature = NCombatRoom.Instance?.GetCreatureNode(hittableEnemy);
            if (nCreature != null)
            {
                NGaseousImpactVfx? child = NGaseousImpactVfx.Create(nCreature.VfxSpawnPosition, new Color("83eb85"));
                NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(child);
            }
        }
        await PowerCmd.Apply<PoisonPower>(enemies, EvokeVal, Owner.Creature, null);
        return enemies;
    }
}
