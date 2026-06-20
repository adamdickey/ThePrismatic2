using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.ValueProps;

namespace ThePrismatic2.ThePrismatic2Code.Orbs;


public sealed class GloomOrb : CustomOrbModel
{
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<DoomPower>());
    
    public override Color DarkenedColor => new("9001d3");
    public override string CustomIconPath => "res://.godot/imported/dark_orb.png-5f06b12b25c362f6d903a63dbe2e565f.ctex";
    public override bool IncludeInRandomPool => true;

    // Reuse Dark Orb sounds - practical use of overrides
    public override string CustomPassiveSfx => "event:/sfx/characters/defect/defect_dark_passive";
    public override string CustomEvokeSfx => "event:/sfx/characters/defect/defect_dark_evoke";
    public override string CustomChannelSfx => "event:/sfx/characters/defect/defect_dark_channel";
    
    private decimal _evokeVal = 4m;
    public override decimal PassiveVal => ModifyOrbValue(4m);

    public override decimal EvokeVal => _evokeVal;

    public override Node2D CreateCustomSprite()
    {
        var container = new Node2D();
        string darkPath = SceneHelper.GetScenePath("orbs/orb_visuals/dark_orb");
        Node2D dark = PreloadManager.Cache.GetScene(darkPath)
            .Instantiate<Node2D>();
        new MegaSprite(dark.GetNode("SpineSkeleton"))
            .GetAnimationState().SetAnimation("idle_loop");
        dark.Modulate = new Color(0.95f, 0.95f, 0.95f);
        dark.Scale = new Vector2(1.0f, 1.0f);
        container.AddChild(dark);
        return container;
    }
    
    public override async Task BeforeTurnEndOrbTrigger(PlayerChoiceContext choiceContext)
    {
        await Passive(choiceContext, null);
    }

    public override Task Passive(PlayerChoiceContext choiceContext, Creature? target)
    {
        if (target != null)
        {
            throw new InvalidOperationException("Gloom orbs cannot target creatures.");
        }
        Trigger();
        _evokeVal += PassiveVal;
        NCombatRoom.Instance?.GetCreatureNode(Owner.Creature)?.OrbManager?.UpdateVisuals(OrbEvokeType.None);
        return Task.CompletedTask;
    }

    public override async Task<IEnumerable<Creature>> Evoke(PlayerChoiceContext playerChoiceContext)
    {
        IReadOnlyList<Creature> hittableEnemies = CombatState.HittableEnemies;
        if (hittableEnemies.Count == 0)
        {
            return Array.Empty<Creature>();
        }
        PlayEvokeSfx();
        Creature? weakestEnemy = hittableEnemies.MinBy(c => c.CurrentHp);
        if (weakestEnemy == null)
        {
            return Array.Empty<Creature>();
        }
        await CreatureCmd.Damage(playerChoiceContext, weakestEnemy, EvokeVal, ValueProp.Unpowered, Owner.Creature);
        await PowerCmd.Apply<DoomPower>(playerChoiceContext, weakestEnemy, EvokeVal, Owner.Creature, null);
        return new _003C_003Ez__ReadOnlySingleElementList<Creature>(weakestEnemy);
    }
}
