using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class ConsumingShadow2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/consuming_shadow_power.png-22a9653c8bff835d2d52a9b619e6826c.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/consuming_shadow_power.png-22a9653c8bff835d2d52a9b619e6826c.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<DoomPower>());

    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side == Owner.Side && Owner.Player is { PlayerCombatState: not null } && Owner.Player.PlayerCombatState.OrbQueue.Orbs.Count != 0)
        {
            for (int i = 0; i < Amount; i++)
            {
                await OrbCmd.EvokeLast(choiceContext, Owner.Player);
                await Cmd.Wait(0.25f);
            }
            await DoomPower.DoomKill(DoomPower.GetDoomedCreatures(CombatState.HittableEnemies));
        }
    }
}
