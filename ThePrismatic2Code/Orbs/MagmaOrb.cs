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


public sealed class MagmaOrb : CustomOrbModel
{
    public override Color DarkenedColor => new("800000");
    public override string CustomIconPath => "res://.godot/imported/dark_orb.png-5f06b12b25c362f6d903a63dbe2e565f.ctex";
    public override bool IncludeInRandomPool => true;

    // Reuse Dark Orb sounds - practical use of overrides
    public override string CustomPassiveSfx => "event:/sfx/characters/defect/defect_dark_passive";
    public override string CustomEvokeSfx => "event:/sfx/characters/defect/defect_dark_evoke";
    public override string CustomChannelSfx => "event:/sfx/characters/defect/defect_dark_channel";

    public override decimal PassiveVal => ModifyOrbValue(2m);
    public override decimal EvokeVal => ModifyOrbValue(4m);

    public override Node2D CreateCustomSprite()
    {
        var container = new Node2D();
        string darkPath = SceneHelper.GetScenePath("orbs/orb_visuals/dark_orb");
        Node2D dark = PreloadManager.Cache.GetScene(darkPath)
            .Instantiate<Node2D>();
        new MegaSprite(dark.GetNode("SpineSkeleton"))
            .GetAnimationState().SetAnimation("idle_loop");
        dark.Modulate = new Color(0.1f, 0.0f, 0.0f);
        container.AddChild(dark);
        string glassPath = SceneHelper.GetScenePath("orbs/orb_visuals/glass_orb");
        Node2D glass = PreloadManager.Cache.GetScene(glassPath)
            .Instantiate<Node2D>();
        new MegaSprite(glass.GetNode("SpineSkeleton"))
            .GetAnimationState().SetAnimation("idle_loop");
        glass.Modulate = new Color(0.9f, 0.0f, 0.0f);
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
        Trigger();
        await ApplyVigor(choiceContext, PassiveVal);
    }

    public override async Task<IEnumerable<Creature>> Evoke(PlayerChoiceContext playerChoiceContext)
    {
        return await ApplyVigor(playerChoiceContext, EvokeVal);
    }

    private async Task<IEnumerable<Creature>> ApplyVigor(PlayerChoiceContext choiceContext, decimal value)
    {
        await PowerCmd.Apply<VigorPower>(choiceContext, Owner.Creature, value, Owner.Creature, null);
        return Array.Empty<Creature>();
    }
}
