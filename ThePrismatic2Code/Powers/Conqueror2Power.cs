using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class Conqueror2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/conqueror_power.png-29235f1270e50ddbe47d2cb18370fdaa.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/conqueror_power.png-29235f1270e50ddbe47d2cb18370fdaa.s3tc.ctex";
    
    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(Extensions.Keywords.Costly)];

    public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (cardSource != null && !(cardSource.EnergyCost.Canonical + Math.Max(0, cardSource.CurrentStarCost) >= 2))
        {
            return 1m;
        }
        if (!props.IsPoweredAttack())
        {
            return 1m;
        }
        if (target != Owner)
        {
            return 1m;
        }
        return 2m;
    }

    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side == Owner.Side)
        {
            await PowerCmd.TickDownDuration(this);
        }
    }
}
