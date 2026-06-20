using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class Smokestack2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/smokestack_power.png-4a759e5609862177427fac54d8f0c5f8.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/smokestack_power.png-4a759e5609862177427fac54d8f0c5f8.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterCardGeneratedForCombat(CardModel card, Player? creator)
    {
        if (creator == Owner.Player && card.Owner.Creature == Owner)
        {
            Flash();
            await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), CombatState.HittableEnemies, Amount, ValueProp.Unpowered, Owner, null);
        }
    }
}
