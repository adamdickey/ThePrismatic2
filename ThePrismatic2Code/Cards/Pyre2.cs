using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;
using ThePrismatic2.ThePrismatic2Code.Orbs;
using ThePrismatic2.ThePrismatic2Code.Powers;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Pyre2() : ThePrismatic2Card(2, 
    CardType.Power, CardRarity.Rare, 
    TargetType.Self)
{
    public override string CustomPortraitPath => "res://.godot/imported/pyre.png-1a39ac5d1be3a8418f22769ccb32b45f.ctex";
    public override string PortraitPath => "res://.godot/imported/pyre.png-1a39ac5d1be3a8418f22769ccb32b45f.ctex";

    protected override IEnumerable<DynamicVar> CanonicalVars => new global::_003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new EnergyVar(1));

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new global::_003C_003Ez__ReadOnlyArray<IHoverTip>(new IHoverTip[3]
    {
        base.EnergyHoverTip,
        HoverTipFactory.Static(StaticHoverTip.Channeling),
        HoverTipFactory.FromOrb<MagmaOrb>()
    });

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<Pyre2Power>(base.Owner.Creature, base.DynamicVars.Energy.BaseValue, base.Owner.Creature, this);
    }

    public override async Task OnEnqueuePlayVfx(Creature? target)
    {
        NFireBurningVfx child = NFireBurningVfx.Create(base.Owner.Creature, 1f, goingRight: false);
        NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(child);
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
    }

    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}