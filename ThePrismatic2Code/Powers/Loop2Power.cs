using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class Loop2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/loop_power.png-110f11d2c0ddc1ac0b0c1cd7c51f4c65.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/loop_power.png-110f11d2c0ddc1ac0b0c1cd7c51f4c65.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new StarsVar(0));
    
    public override Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (power == this)
        {
            DynamicVars.Stars.UpgradeValueBy(1m);
        }
        return Task.CompletedTask;
    }

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player == Owner.Player && player.PlayerCombatState != null)
        {
            await PlayerCmd.GainStars(DynamicVars.Stars.BaseValue, Owner.Player);
            if (player.PlayerCombatState.OrbQueue.Orbs.Count != 0)
            {
                for (int i = 0; i < Amount; i++)
                {
                    await OrbCmd.Passive(choiceContext, player.PlayerCombatState.OrbQueue.Orbs[0], null);
                    await Cmd.Wait(0.25f);
                }
            }
        }
    }
}
