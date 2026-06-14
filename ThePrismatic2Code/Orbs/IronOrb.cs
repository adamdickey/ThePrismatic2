using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;

namespace ThePrismatic2.ThePrismatic2Code.Orbs;


public sealed class IronOrb : CustomOrbModel
{
    public override Color DarkenedColor => new("ffff00");
    public override string CustomIconPath => "res://ThePrismatic2/images/orbs/solar_orb.png";
    public override bool IncludeInRandomPool => true;

    // Reuse Dark Orb sounds - practical use of overrides
    public override string CustomPassiveSfx => "event:/sfx/characters/defect/defect_dark_passive";
    public override string CustomEvokeSfx => "event:/sfx/characters/defect/defect_dark_evoke";
    public override string CustomChannelSfx => "event:/sfx/characters/defect/defect_dark_channel";

    public override decimal PassiveVal => ModifyOrbValue(2m);
    public override decimal EvokeVal => ModifyOrbValue(5m);

    public override Node2D CreateCustomSprite()
    {
        var container = new Node2D();
        string darkPath = SceneHelper.GetScenePath("orbs/orb_visuals/dark_orb");
        Node2D dark = PreloadManager.Cache.GetScene(darkPath)
            .Instantiate<Node2D>();
        new MegaSprite(dark.GetNode("SpineSkeleton"))
            .GetAnimationState().SetAnimation("idle_loop");
        dark.Modulate = new Color(0.20f, 0.20f, 0.19f);
        dark.Scale = new Vector2(1.1f, 1.1f);
        container.AddChild(dark);
        string glassPath = SceneHelper.GetScenePath("orbs/orb_visuals/glass_orb");
        Node2D glass = PreloadManager.Cache.GetScene(glassPath)
            .Instantiate<Node2D>();
        new MegaSprite(glass.GetNode("SpineSkeleton"))
            .GetAnimationState().SetAnimation("idle_loop");
        glass.Modulate = new Color(0.38f, 0.4f, 0.42f);
        container.AddChild(glass);
        return container;
    }
    
    public override async Task BeforeTurnEndOrbTrigger(PlayerChoiceContext choiceContext)
    {
        await Passive(choiceContext, null);
    }

    public override async Task Passive(PlayerChoiceContext choiceContext, Creature? target)
    {
        Trigger();
        await ForgeCmd.Forge(PassiveVal, Owner, this);
    }

    public override async Task<IEnumerable<Creature>> Evoke(PlayerChoiceContext playerChoiceContext)
    {
        await ForgeCmd.Forge(EvokeVal, Owner, this);
        return Array.Empty<Creature>();
    }
}
