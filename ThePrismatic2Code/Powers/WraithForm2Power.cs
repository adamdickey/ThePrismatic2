using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class WraithForm2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/wraith_form_power.png-2267bc74587e36e5535b57176492d48e.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/wraith_form_power.png-2267bc74587e36e5535b57176492d48e.s3tc.ctex";
    
    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<DoomPower>());

    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (participants.Contains(Owner))
        {
            await PowerCmd.Apply<DoomPower>(new ThrowingPlayerChoiceContext(), Owner, Amount, Owner, null);
        }
    }
}
