using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class Veilpiercer2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/veilpiercer_power.png-8ce2bf07f7a5df9cdaffea4815d67765.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/veilpiercer_power.png-8ce2bf07f7a5df9cdaffea4815d67765.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        HoverTipFactory.FromKeyword(CardKeyword.Ethereal),
        HoverTipFactory.FromKeyword(Extensions.Keywords.Cunning)
        ]);

    public override bool TryModifyEnergyCostInCombat(CardModel card, decimal originalCost, out decimal modifiedCost)
    {
        modifiedCost = originalCost;
        if (card.Owner.Creature != Owner)
        {
            return false;
        }
        if (!(card.Keywords.Contains(CardKeyword.Ethereal) || card.Keywords.Contains(Extensions.Keywords.Cunning)))
        {
            return false;
        }

        bool flag = card.Pile?.Type switch
        {
            PileType.Hand or PileType.Play => true,
            _ => false
        };
        if (!flag)
        {
            return false;
        }
        modifiedCost = 0;
        return true;
    }

    public override async Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature == Owner && (cardPlay.Card.Keywords.Contains(CardKeyword.Ethereal) || cardPlay.Card.Keywords.Contains(Extensions.Keywords.Cunning)))
        {
            bool flag = cardPlay.Card.Pile?.Type switch
            {
                PileType.Hand or PileType.Play => true,
                _ => false
            };
            if (flag)
            {
                await PowerCmd.Decrement(this);
            }
        }
    }
}
