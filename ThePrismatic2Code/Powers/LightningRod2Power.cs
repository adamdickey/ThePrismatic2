using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Orbs;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class LightningRod2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/lightning_rod_power.png-39ee804869c27e9e684225e058c1921d.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/lightning_rod_power.png-39ee804869c27e9e684225e058c1921d.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        HoverTipFactory.Static(StaticHoverTip.Channeling),
        HoverTipFactory.FromOrb<LightningOrb>()
    ]);
    
    public override async Task AfterEnergyReset(Player player)
    {
        if (player == Owner.Player)
        {
            await PlayerCmd.GainStars(1, Owner.Player);
            await OrbCmd.Channel<LightningOrb>(new ThrowingPlayerChoiceContext(), Owner.Player);
            await PowerCmd.Decrement(this);
        }
    }
}
