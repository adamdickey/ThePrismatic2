using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class AnticipateFocusPower : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/anticipate_power.png-f04687be18ba16beaa4d734d20d91615.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/anticipate_power.png-f04687be18ba16beaa4d734d20d91615.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override bool AllowNegative => true;
    
    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (side == Owner.Side)
        {
            Flash();
            await PowerCmd.Remove(this);
            await PowerCmd.Apply<FocusPower>(choiceContext, Owner, -Amount, Owner, null);
        }
    }
    
    
}
