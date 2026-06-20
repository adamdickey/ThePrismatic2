using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class Outbreak2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/outbreak_power.png-fe094ab03bd08b0f9f2e509092082798.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/outbreak_power.png-fe094ab03bd08b0f9f2e509092082798.s3tc.ctex";
    
    private class Data
    {
        public int TimesPoisoned;
    }

    public const int PoisonThreshold = 3;

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override int DisplayAmount => GetInternalData<Data>().TimesPoisoned;

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new RepeatVar(3));

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
            HoverTipFactory.FromPower<PoisonPower>(),
            HoverTipFactory.FromPower<DoomPower>()
            ]);

    protected override object InitInternalData()
    {
        return new Data();
    }

    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (applier == Owner && !(amount <= 0m) && power is PoisonPower or DoomPower)
        {
            Data data = GetInternalData<Data>();
            data.TimesPoisoned++;
            if (data.TimesPoisoned >= 3)
            {
                InvokeDisplayAmountChanged();
                Flash();
                if (Owner.CombatState != null)
                    await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), Owner.CombatState.HittableEnemies,
                        Amount, ValueProp.Unpowered, Owner, null);
                data.TimesPoisoned %= 3;
            }
            InvokeDisplayAmountChanged();
        }
    }
}
