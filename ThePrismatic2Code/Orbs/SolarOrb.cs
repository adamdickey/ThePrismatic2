using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;

namespace ThePrismatic2.ThePrismatic2Code.Orbs;


public sealed class SolarOrb : CustomOrbModel
{
    public override Color DarkenedColor => new Color("ffff00");
    public override string CustomIconPath => "res://ThePrismatic2/images/orbs/solar_orb.png";
    public override bool IncludeInRandomPool => true;

    // Reuse Dark Orb sounds - practical use of overrides
    public override string CustomPassiveSfx => "event:/sfx/characters/defect/defect_dark_passive";
    public override string CustomEvokeSfx => "event:/sfx/characters/defect/defect_dark_evoke";
    public override string CustomChannelSfx => "event:/sfx/characters/defect/defect_dark_channel";

    public override decimal PassiveVal => ModifyOrbValue(1m);
    public override decimal EvokeVal => ModifyOrbValue(2m);

    public override Node2D CreateCustomSprite()
    {
        var container = new Node2D();
        string glassPath = SceneHelper.GetScenePath("orbs/orb_visuals/glass_orb");
        Node2D glass = PreloadManager.Cache.GetScene(glassPath)
            .Instantiate<Node2D>();
        new MegaSprite(glass.GetNode("SpineSkeleton"))
            .GetAnimationState().SetAnimation("idle_loop");
        glass.Modulate = new Color(1.0f, 0.75f, 0.0f);
        container.AddChild(glass);
        return container;
    }
    
    public override async Task Passive(PlayerChoiceContext choiceContext, Creature? target)
    {
        Trigger();
        await AfterOrbChanneled(choiceContext, null, this);
    }

    public override async Task AfterOrbChanneled(PlayerChoiceContext choiceContext, Player? player, OrbModel orb)
    {
        if (orb == this)
        {
            Trigger();
            await PlayerCmd.GainStars(PassiveVal, Owner);
        }
    }
    
    public override async Task<IEnumerable<Creature>> Evoke(PlayerChoiceContext choiceContext)
    {
        await PlayerCmd.GainStars(EvokeVal, Owner);
        return new _003C_003Ez__ReadOnlySingleElementList<Creature>(Owner.Creature);
    }
}
