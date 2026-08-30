using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class MonarchsGaze2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/monarchs_gaze_power.png-a1da396cd508771a43168b8e06c3d469.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/monarchs_gaze_power.png-a1da396cd508771a43168b8e06c3d469.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterDamageGiven(PlayerChoiceContext choiceContext, Creature? dealer, DamageResult _, ValueProp props, Creature target, CardModel? cardSource)
    {
        if (Owner.Player != null && (dealer == Owner || dealer == Owner.Player.Osty) && props.IsPoweredAttack())
        {
            await PowerCmd.Apply<MonarchsGaze2StrengthDownPower>(choiceContext, target, Amount, Owner, null);
        }
    }
}
