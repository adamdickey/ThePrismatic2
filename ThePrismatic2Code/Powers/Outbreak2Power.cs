using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class Outbreak2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/outbreak_power.png-fe094ab03bd08b0f9f2e509092082798.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/outbreak_power.png-fe094ab03bd08b0f9f2e509092082798.s3tc.ctex";

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (applier == Owner && power.Owner != Owner && !(amount <= 0m) && power.Type == PowerType.Debuff)
        {
            Flash();
            if (Owner.CombatState != null)
                await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), Owner.CombatState.HittableEnemies, Amount, ValueProp.Unpowered, Owner, null);
        }
    }
}
