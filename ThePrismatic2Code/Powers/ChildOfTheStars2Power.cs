using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class ChildOfTheStars2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/child_of_the_stars_power.png-467f26f0e915158114fb043109f297a1.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/child_of_the_stars_power.png-467f26f0e915158114fb043109f297a1.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterStarsSpent(int amount, Player spender)
    {
        if (amount > 0 && spender == Owner.Player)
        {
            Flash();
            await CreatureCmd.GainBlock(Owner, Amount * amount, ValueProp.Unpowered, null);
        }
    }
    
    public override async Task AfterOrbEvoked(PlayerChoiceContext choiceContext, OrbModel orb, IEnumerable<Creature> targets)
    {
        Flash();
        await CreatureCmd.GainBlock(Owner, Amount, ValueProp.Unpowered, null);
    }
}
