using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class Vicious2Power : ThePrismatic2Power
{
    
    public override string CustomPackedIconPath => "res://.godot/imported/vicious_power.png-29e262cf80ea8b0751edcb4a119b2556.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/vicious_power.png-29e262cf80ea8b0751edcb4a119b2556.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new global::_003C_003Ez__ReadOnlyArray<IHoverTip>(
        new IHoverTip[3]
        {
            HoverTipFactory.FromPower<WeakPower>(),
            HoverTipFactory.FromPower<VulnerablePower>(),
            HoverTipFactory.FromPower<ExposedPower>()
        });

    public override async Task AfterPowerAmountChanged(PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (!(amount <= 0m) && applier == base.Owner && (power is VulnerablePower || power is WeakPower || power is ExposedPower))
        {
            Flash();
            await CardPileCmd.Draw(new BlockingPlayerChoiceContext(), base.Amount, base.Owner.Player);
        }
    }
}
