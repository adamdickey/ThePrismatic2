using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class DemonForm2Power : ThePrismatic2Power
{
    
    public override string CustomPackedIconPath => "res://.godot/imported/demon_form_power.png-ee781e7a72a5f65b71097c5c7ba4c564.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/demon_form_power.png-ee781e7a72a5f65b71097c5c7ba4c564.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<StrengthPower>());

    public override async Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    {
        if (side == Owner.Side)
        {
            Flash();
            await PowerCmd.Apply<StrengthPower>(Owner, Amount, Owner, null);
            await PowerCmd.Apply<CalcifyPower>(Owner, Amount, Owner, null);
        }
    }
}
