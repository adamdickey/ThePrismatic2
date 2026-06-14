using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class Haunt2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/haunt_power.png-2c56457cd24d7d8120da05eaadf28298.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/haunt_power.png-2c56457cd24d7d8120da05eaadf28298.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.EnergyCost.GetResolved() == 0 && cardPlay.Card.Owner.Creature == Owner)
        {
            IReadOnlyList<Creature> hittableEnemies = CombatState.HittableEnemies;
            if (hittableEnemies.Count != 0)
            {
                Creature? item = Owner.Player?.RunState.Rng.CombatTargets.NextItem(hittableEnemies);
                if (item != null)
                    await CreatureCmd.Damage(context, new _003C_003Ez__ReadOnlySingleElementList<Creature>(item),
                        Amount, ValueProp.Unblockable | ValueProp.Unpowered, null, null);
            }
        }
    }
}
