using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class Spinner2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/spinner_power.png-3dd26976f0f4be73cd17fdc85404c26e.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/spinner_power.png-3dd26976f0f4be73cd17fdc85404c26e.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Channeling));

    public override async Task AfterEnergyReset(Player player)
    {
        if (player == Owner.Player)
        {
            for (int i = 0; i < Amount; i++)
            {
                await OrbCmd.Channel(new ThrowingPlayerChoiceContext(), OrbModel.GetRandomOrb(Owner.Player.RunState.Rng.CombatOrbGeneration).ToMutable(), Owner.Player);
            }
        }
    }
}
