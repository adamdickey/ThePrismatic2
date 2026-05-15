using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;
using ThePrismatic2.ThePrismatic2Code.Powers;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Unmovable2() : ThePrismatic2Card(2, 
    CardType.Power, CardRarity.Rare, 
    TargetType.Self)
{
    public override string CustomPortraitPath => "res://.godot/imported/unmovable.png-2a501a9619b6d09a82ac58cd84f1eddb.ctex";
    public override string PortraitPath => "res://.godot/imported/unmovable.png-2a501a9619b6d09a82ac58cd84f1eddb.ctex";

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new global::_003C_003Ez__ReadOnlyArray<IHoverTip>(
        new IHoverTip[2]
        {
            HoverTipFactory.Static(StaticHoverTip.Block),
            HoverTipFactory.Static(StaticHoverTip.SummonStatic)
        });

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        NPowerUpVfx.CreateNormal(base.Owner.Creature);
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<Unmovable2Power>(base.Owner.Creature, 1m, base.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}