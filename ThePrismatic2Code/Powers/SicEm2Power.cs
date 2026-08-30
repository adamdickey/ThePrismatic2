using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class SicEm2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/sic_em_power.png-08c4e9290cf0e446d99cc67ad46c002c.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/sic_em_power.png-08c4e9290cf0e446d99cc67ad46c002c.s3tc.ctex";
    
    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.SummonStatic));
    
    public override async Task AfterDamageGiven(PlayerChoiceContext choiceContext, Creature? dealer, DamageResult result, ValueProp props, Creature target, CardModel? cardSource)
    {
        if (dealer?.Monster is Osty osty && osty.Creature.PetOwner != null && Applier != null && osty.Creature.PetOwner.Creature == Applier && target == Owner)
        {
            if (dealer.PetOwner != null) await OstyCmd.Summon(choiceContext, dealer.PetOwner, Amount, this);
        }
    }
    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (amount != 0m && power.GetTypeForAmount(amount) == PowerType.Debuff && power.Owner == Owner && applier != Owner && !(power is ITemporaryPower))
        {
            decimal summonAmount = power == this ? Amount - amount : Amount;
            if (summonAmount <= 0) return;
            if (Applier?.Player != null) await OstyCmd.Summon(choiceContext, Applier.Player, summonAmount, this);
        }
    }

    public override async Task AfterSideTurnStartLate(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (side == Owner.Side)
        {
            await PowerCmd.Remove(this);
        }
    }
}
