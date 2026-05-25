using System.Runtime.InteropServices.JavaScript;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
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
    private bool _canDraw = true;
    private List<Creature> _targets = [];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>(
        new IHoverTip[3]
        {
            HoverTipFactory.FromPower<WeakPower>(),
            HoverTipFactory.FromPower<VulnerablePower>(),
            HoverTipFactory.FromPower<ExposedPower>()
        });

    public override Task BeforeCardPlayed(CardPlay cardPlay)
    {
        _targets = [];
        return Task.CompletedTask;
    }
    
    public override Task BeforePowerAmountChanged(PowerModel power, decimal amount, Creature target, Creature? applier, CardModel? cardSource)
    {
        _canDraw = !_targets.Contains(target);
        if (_canDraw)
        {
            _targets.Add(target);
        }
        return Task.CompletedTask;
    }

    public override async Task AfterPowerAmountChanged(PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (!(amount <= 0m) && _canDraw && applier == Owner && (power is VulnerablePower || power is WeakPower || power is ExposedPower))
        {
            Flash();
            if (Owner.Player != null) await CardPileCmd.Draw(new BlockingPlayerChoiceContext(), Amount, Owner.Player);
            _canDraw = false;
        }
    }
}
