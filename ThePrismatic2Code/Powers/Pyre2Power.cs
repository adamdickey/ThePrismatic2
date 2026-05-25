using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Orbs;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class Pyre2Power : ThePrismatic2Power
{
    
    public override string CustomPackedIconPath => "res://.godot/imported/pyre_power.png-56891c5f1c4364738c2a096fd55490c2.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/pyre_power.png-56891c5f1c4364738c2a096fd55490c2.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>(new IHoverTip[3]
    {
        HoverTipFactory.ForEnergy(this),
        HoverTipFactory.Static(StaticHoverTip.Channeling),
        HoverTipFactory.FromOrb<MagmaOrb>()
    });

    public override async Task AfterEnergyReset(Player player)
    {
        if (player == Owner.Player)
        {
            for (int i = 0; i < Amount; i++)
            {
                await OrbCmd.Channel<MagmaOrb>(new ThrowingPlayerChoiceContext(), Owner.Player);
            }
        }
    }

    public override decimal ModifyMaxEnergy(Player player, decimal amount)
    {
        if (player != Owner.Player)
        {
            return amount;
        }
        return amount + (decimal)Amount;
    }
}
