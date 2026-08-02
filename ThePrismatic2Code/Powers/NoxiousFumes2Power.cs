using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using ThePrismatic2.ThePrismatic2Code.Orbs;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class NoxiousFumes2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/noxious_fumes_power.png-48b7e2631c623fd70ff5141ae353ee77.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/noxious_fumes_power.png-48b7e2631c623fd70ff5141ae353ee77.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        HoverTipFactory.Static(StaticHoverTip.Channeling),
        HoverTipFactory.FromOrb<VenomOrb>()
    ]);

    public override async Task AfterEnergyReset(Player player)
    {
        Flash();
        if (player == Owner.Player)
        {
            for (int i = 0; i < Amount; i++)
            {
                await OrbCmd.Channel<VenomOrb>(new ThrowingPlayerChoiceContext(), Owner.Player);
            }
        }
    }
}
