using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Powers;

namespace ThePrismatic2.ThePrismatic2Code.Orbs;


public sealed class VenomOrb : CustomOrbModel
{
    public override Color DarkenedColor => new Color("2d6e2d");
    public override string? CustomIconPath => "res://ThePrismatic2/images/orbs/venom_orb.png";
    public override bool IncludeInRandomPool => true;

    // Reuse Dark Orb sounds - practical use of overrides
    public override string? CustomPassiveSfx => "event:/sfx/characters/defect/defect_dark_passive";
    public override string? CustomEvokeSfx => "event:/sfx/characters/defect/defect_dark_evoke";
    public override string? CustomChannelSfx => "event:/sfx/characters/defect/defect_dark_channel";

    private decimal _turnsLeft = 3m;
    private decimal _passiveVal = 1m;
    public override decimal PassiveVal => ModifyOrbValue(_passiveVal);
    public override decimal EvokeVal => _turnsLeft;

    public override Node2D? CreateCustomSprite()
    {
        var container = new Node2D();
        // back layer: dark orb (green tint)
        string darkPath = SceneHelper.GetScenePath("orbs/orb_visuals/dark_orb");
        Node2D dark = PreloadManager.Cache.GetScene(darkPath)
            .Instantiate<Node2D>(PackedScene.GenEditState.Disabled);
        new MegaSprite(dark.GetNode("SpineSkeleton"))
            .GetAnimationState().SetAnimation("idle_loop");
        dark.Modulate = _passiveVal <= 0m ? new Color(0.0f, 0.1f, 0.0f, 1.0f) : new Color(0.1f, 0.5f, 0.1f, 1.0f);
        dark.Scale = new Vector2(1.1f, 1.1f);
        container.AddChild(dark);
        // front layer: glass orb (bright green core)
        string glassPath = SceneHelper.GetScenePath("orbs/orb_visuals/glass_orb");
        Node2D glass = PreloadManager.Cache.GetScene(glassPath)
            .Instantiate<Node2D>(PackedScene.GenEditState.Disabled);
        new MegaSprite(glass.GetNode("SpineSkeleton"))
            .GetAnimationState().SetAnimation("idle_loop");
        glass.Modulate = _passiveVal <= 0m ? new Color(0.1f, 0.3f, 0.1f, 1.0f) : new Color(0.3f, 0.9f, 0.3f, 1.0f);
        //glass.Modulate = new Color(0.3f, 0.9f, 0.3f, 1.0f);
        container.AddChild(glass);
        return container;
    }

    public override async Task BeforeTurnEndOrbTrigger(PlayerChoiceContext choiceContext)
        => await Passive(choiceContext, null);
    
    public override async Task Passive(PlayerChoiceContext choiceContext, Creature? target)
    {
        List<Creature> targets = base.CombatState.HittableEnemies.Where((Creature e) => e.IsHittable).ToList();
        decimal turnsLeft = _turnsLeft;
        if (!(turnsLeft <= 0m))
        {
            Trigger();
            PlayPassiveSfx();
            _turnsLeft = Math.Max(0m, _turnsLeft - 1m);
            await PowerCmd.Apply<PoisonPower>(targets, PassiveVal, Owner.Creature, null);
        }
        else _passiveVal = 0;
    }
    
    public override async Task<IEnumerable<Creature>> Evoke(PlayerChoiceContext choiceContext)
    {
        List<Creature> enemies = base.CombatState.HittableEnemies.Where((Creature e) => e.IsHittable).ToList();
        if (EvokeVal <= 0m)
        {
            return Array.Empty<Creature>();
        }
        await PowerCmd.Apply<PoisonPower>(enemies, PassiveVal, Owner.Creature, null);
        return enemies;
    }
}
