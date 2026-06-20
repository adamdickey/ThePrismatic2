using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class TrashToTreasure2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/trash_to_treasure_power.png-7dc93da4eaba0ac95f6ef2b1059c2ef7.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/trash_to_treasure_power.png-7dc93da4eaba0ac95f6ef2b1059c2ef7.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterCardGeneratedForCombat(CardModel card, Player? creator)
    {
        if (creator == Owner.Player && card.Owner.Creature == Owner)
        {
            Flash();
            for (int i = 0; i < Amount; i++)
            {
                if (Owner.Player != null)
                {
                    OrbModel orb = OrbModel.GetRandomOrb(Owner.Player.RunState.Rng.CombatOrbGeneration).ToMutable();
                    await OrbCmd.Channel(new ThrowingPlayerChoiceContext(), orb, Owner.Player);
                }
            }
        }
    }
}
