using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class BiasedCognition2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/biased_cognition_power.png-68133af0b2a79717f45ae490da21d3d8.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/biased_cognition_power.png-68133af0b2a79717f45ae490da21d3d8.s3tc.ctex";
    
    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        HoverTipFactory.FromPower<FocusPower>(),
        HoverTipFactory.FromPower<StrengthPower>()
    ]);

    public override async Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    {
        if (side == Owner.Side)
        {
            Flash();
            await PowerCmd.Apply<FocusPower>(Owner, -Amount, Owner, null);
            await PowerCmd.Apply<StrengthPower>(Owner, -Amount, Owner, null);
        }
    }
}
