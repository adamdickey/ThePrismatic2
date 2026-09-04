using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using ThePrismatic2.ThePrismatic2Code.Extensions;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class OchoFormPower : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "OchOForm.png".PowerImagePath();
    public override string CustomBigIconPath => "OchOForm.png".BigPowerImagePath();
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override int ModifyCardPlayCount(CardModel card, Creature? target, int playCount)
    {
        if (card.Owner.Creature != Owner)
        {
            return playCount;
        }
        int num = CombatManager.Instance.History.CardPlaysStarted.Count(e => e.Actor == Owner && e.CardPlay.IsFirstInSeries && e.HappenedThisTurn(CombatState) && e.CardPlay.Resources.EnergyValue == 0);
        if (num >= Amount || card.EnergyCost.GetAmountToSpend() != 0)
        {
            return playCount;
        }
        return playCount + 1;
    }

    public override Task AfterModifyingCardPlayCount(CardModel card)
    {
        Flash();
        return Task.CompletedTask;
    }
}
