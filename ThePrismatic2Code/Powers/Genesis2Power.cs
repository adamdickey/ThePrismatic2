using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using ThePrismatic2.ThePrismatic2Code.Orbs;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class Genesis2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/genesis_power.png-fbb5fcea8f8c7afebbb2cc171c10c2d4.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/genesis_power.png-fbb5fcea8f8c7afebbb2cc171c10c2d4.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        HoverTipFactory.Static(StaticHoverTip.Channeling),
        HoverTipFactory.FromOrb<SolarOrb>()
    ]);
    
    public override async Task AfterEnergyReset(Player player)
    {
        if (player == Owner.Player)
        {
            Flash();
            for (int i = 0; i < Amount; i++)
            {
                await OrbCmd.Channel<SolarOrb>(new BlockingPlayerChoiceContext(), player);
            }
        }
    }
}
