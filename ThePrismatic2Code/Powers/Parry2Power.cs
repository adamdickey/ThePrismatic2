using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class Parry2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/parry_power.png-31b6bce24b765840fbed23c85afd4d4d.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/parry_power.png-31b6bce24b765840fbed23c85afd4d4d.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Type == CardType.Attack && cardPlay.Card.EnergyCost.GetResolved() + Math.Max(0, cardPlay.Card.LastStarsSpent) >= 2)
        {
            Flash();
            await CreatureCmd.GainBlock(cardPlay.Card.Owner.Creature, Amount, ValueProp.Unpowered, null);
        }
    }
}
