using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class Coolant2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/coolant_power.png-1f9369e6fab08d7b02e45bd60cd137fe.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/coolant_power.png-1f9369e6fab08d7b02e45bd60cd137fe.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block));

    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (side == Owner.Side)
        {
            Flash();
            int num = (from orb in Owner.Player?.PlayerCombatState?.OrbQueue.Orbs group orb by orb.Id).Count();
            List<ModelId> debuffs = new();
            foreach (Creature enemy in CombatState.HittableEnemies)
            {
                foreach (PowerModel power in enemy.Powers)
                {
                    if (power.Type == PowerType.Debuff && !debuffs.Contains(power.Id))
                    {
                        debuffs.Add(power.Id);
                    }
                }
            }
            num += debuffs.Count;
            await CreatureCmd.GainBlock(Owner, num * Amount, ValueProp.Unpowered, null);
        }
    }
}
