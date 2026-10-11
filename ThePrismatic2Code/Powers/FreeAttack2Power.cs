using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class FreeAttack2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/free_attack_power.png-f95bdbdee65b825e45e00eb5d22cbb3b.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/free_attack_power.png-f95bdbdee65b825e45e00eb5d22cbb3b.s3tc.ctex";

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override bool TryModifyEnergyCostInCombatLate(CardModel card, decimal originalCost, out decimal modifiedCost)
    {
        modifiedCost = originalCost;
        if (card.Owner.Creature != Owner)
        {
            return false;
        }
        if (card.Type != CardType.Attack)
        {
            return false;
        }
        bool flag;
        switch (card.Pile?.Type)
        {
            case PileType.Hand:
            case PileType.Play:
                flag = true;
                break;
            default:
                flag = false;
                break;
        }
        if (!flag)
        {
            return false;
        }
        modifiedCost = 0;
        return true;
    }
    
    public override bool TryModifyStarCost(CardModel card, decimal originalCost, out decimal modifiedCost)
    {
        modifiedCost = originalCost;
        if (card.Owner.Creature != Owner)
        {
            return false;
        }
        if (card.Type != CardType.Attack)
        {
            return false;
        }
        bool flag;
        switch (card.Pile?.Type)
        {
            case PileType.Hand:
            case PileType.Play:
                flag = true;
                break;
            default:
                flag = false;
                break;
        }
        if (!flag)
        {
            return false;
        }
        modifiedCost = 0;
        return true;
    }

    public override async Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature == Owner && cardPlay.Card.Type == CardType.Attack)
        {
            bool flag;
            switch (cardPlay.Card.Pile?.Type)
            {
                case PileType.Hand:
                case PileType.Play:
                    flag = true;
                    break;
                default:
                    flag = false;
                    break;
            }
            if (flag)
            {
                await PowerCmd.Decrement(this);
            }
        }
    }
}
