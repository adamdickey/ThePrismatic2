using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class SwordSage2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/sword_sage_power.png-42fb345bfc32268a2e3c92145734a827.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/sword_sage_power.png-42fb345bfc32268a2e3c92145734a827.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override int ModifyCardPlayCount(CardModel card, Creature? target, int playCount)
    {
        if (card.Owner.Creature != Owner)
        {
            return playCount;
        }
        if (card.Type != CardType.Attack || card.EnergyCost.GetResolved() + card.LastStarsSpent < 2)
        {
            return playCount;
        }
        return playCount + Amount;
    }

    public override Task AfterModifyingCardPlayCount(CardModel card)
    {
        Flash();
        return Task.CompletedTask;
    }
}
