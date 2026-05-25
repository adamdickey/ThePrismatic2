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
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using ThePrismatic2.ThePrismatic2Code.Character;
using ThePrismatic2.ThePrismatic2Code.Orbs;
using ThePrismatic2.ThePrismatic2Code.Powers;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Haze2() : ThePrismatic2Card(3, 
    CardType.Skill, CardRarity.Uncommon, 
    TargetType.AllEnemies)
{
    public override string CustomPortraitPath => "res://.godot/imported/haze.png-5aa9d2c9549ea2b0f9f025b4fd0d9364.ctex";
    public override string PortraitPath => "res://.godot/imported/haze.png-5aa9d2c9549ea2b0f9f025b4fd0d9364.ctex";

    public override IEnumerable<CardKeyword> CanonicalKeywords => new _003C_003Ez__ReadOnlySingleElementList<CardKeyword>(Extensions.Keywords.Cunning);

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Focus", 2m));

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        HoverTipFactory.Static(StaticHoverTip.Channeling),
        HoverTipFactory.FromOrb<VenomOrb>()
    ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay? cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        SpawnVfx();
        await Cmd.CustomScaledWait(0.2f, 0.4f);
        await PowerCmd.Apply<Haze2Power>(Owner.Creature, DynamicVars["Focus"].BaseValue, Owner.Creature, this);
        await OrbCmd.Channel<VenomOrb>(choiceContext, Owner);
    }

    private void SpawnVfx()
    {
        Node node = NCombatRoom.Instance?.CombatVfxContainer;
        if (node == null)
        {
            return;
        }
        NSmokyVignetteVfx child = NSmokyVignetteVfx.Create(new Color(0.8f, 0.8f, 0.3f, 0.66f), new Color(0f, 4f, 0f, 0.33f));
        node.AddChildSafely(child);
        foreach (Creature hittableEnemy in CombatState.HittableEnemies)
        {
            node.AddChildSafely(NSmokePuffVfx.Create(hittableEnemy, NSmokePuffVfx.SmokePuffColor.Green));
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Focus"].UpgradeValueBy(1m);
    }
}