using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class Juggernaut2Power : ThePrismatic2Power
{
    
    public override string CustomPackedIconPath => "res://.godot/imported/juggernaut_power.png-6ef419c3ac7ccfe7ef270cad45a27420.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/juggernaut_power.png-6ef419c3ac7ccfe7ef270cad45a27420.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterBlockGained(Creature creature, decimal amount, ValueProp props, CardModel? cardSource)
    {
        if (!(amount <= 0m) && creature == base.Owner)
        {
            IReadOnlyList<Creature> hittableEnemies = base.CombatState.HittableEnemies;
            if (hittableEnemies.Count != 0)
            {
                Creature target = base.Owner.Player.RunState.Rng.CombatTargets.NextItem(hittableEnemies);
                Flash();
                await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), target, base.Amount, ValueProp.Unpowered, base.Owner, null);
            }
        }
    }
    
    public override Task AfterSummon(PlayerChoiceContext choiceContext, Player summoner, decimal amount)
    {
        if (!(amount <= 0m) && summoner == base.Owner.Player)
        {
            IReadOnlyList<Creature> hittableEnemies = base.CombatState.HittableEnemies;
            if (hittableEnemies.Count != 0)
            {
                Creature target = base.Owner.Player.RunState.Rng.CombatTargets.NextItem(hittableEnemies);
                Flash();
                CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), target, base.Amount, ValueProp.Unpowered, base.Owner, null);
            }
        }
        return Task.CompletedTask;
    }
}
