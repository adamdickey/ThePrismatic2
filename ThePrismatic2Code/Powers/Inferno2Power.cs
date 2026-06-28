using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class Inferno2Power : ThePrismatic2Power
{
    
    public override string CustomPackedIconPath => "res://.godot/imported/inferno_power.png-688562ad1f0eac4c8a606e5843a12d1b.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/inferno_power.png-688562ad1f0eac4c8a606e5843a12d1b.s3tc.ctex";

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new DamageVar("SelfDamage", 0m, ValueProp.Unblockable | ValueProp.Unpowered));

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player == Owner.Player)
        {
            if (!Osty.CheckMissingWithAnim(player) && player.Osty != null)
            {
                NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(NFireSmokePuffVfx.Create(player.Osty));
                await Cmd.CustomScaledWait(0.2f, 0.4f);
                DamageVar damageVar = (DamageVar)DynamicVars["SelfDamage"];
                await CreatureCmd.Damage(choiceContext, player.Osty, damageVar.BaseValue, damageVar.Props, Owner, null);
            }
            else
            {
                NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(NFireSmokePuffVfx.Create(Owner));
                await Cmd.CustomScaledWait(0.2f, 0.4f);
                DamageVar damageVar = (DamageVar)DynamicVars["SelfDamage"];
                await CreatureCmd.Damage(choiceContext, Owner, damageVar.BaseValue, damageVar.Props, Owner, null);
            }
            
        }
    }
    
    public override async Task AfterCurrentHpChanged(Creature creature, decimal delta)
    {
        if (!(delta >= 0m) && creature.Monster is Osty && creature.PetOwner == Owner.Player && Owner.CombatState != null && Owner.CombatState.CurrentSide == Owner.Side)
        {
            foreach (Creature hittableEnemy in CombatState.HittableEnemies)
            {
                NFireBurstVfx? child = NFireBurstVfx.Create(hittableEnemy, 0.75f);
                NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(child);
            }
            await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), CombatState.HittableEnemies, Amount, ValueProp.Unpowered, Owner, null);
        }
    }

    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (Owner.CombatState != null && (target != Owner || result.UnblockedDamage <= 0 || Owner.CombatState.CurrentSide != Owner.Side))
        {
            return;
        }
        foreach (Creature hittableEnemy in CombatState.HittableEnemies)
        {
            NFireBurstVfx? child = NFireBurstVfx.Create(hittableEnemy, 0.75f);
            NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(child);
        }
        await CreatureCmd.Damage(choiceContext, CombatState.HittableEnemies, Amount, ValueProp.Unpowered, Owner, null);
    }

    public void IncrementSelfDamage()
    {
        AssertMutable();
        DynamicVars["SelfDamage"].BaseValue++;
    }
}
