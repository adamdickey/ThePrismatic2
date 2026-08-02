using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class Calcify2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/calcify_power.png-83482b1c81d634e402c20c2e10f26e20.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/calcify_power.png-83482b1c81d634e402c20c2e10f26e20.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(Extensions.Keywords.DualWield));
    
    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (participants.Contains(Owner) && Owner.Player != null)
        {
            await OstyCmd.Summon(new ThrowingPlayerChoiceContext(), Owner.Player, Amount, this);
        }
    }
}
