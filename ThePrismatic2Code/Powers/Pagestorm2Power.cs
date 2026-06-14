using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class Pagestorm2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/pagestorm_power.png-a73b722e176c28fe44e22f3d6d5f9873.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/pagestorm_power.png-a73b722e176c28fe44e22f3d6d5f9873.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Ethereal));

    public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if (card.Owner.Creature == Owner && (card.Keywords.Contains(CardKeyword.Ethereal) || card.Type == CardType.Power))
        {
            Flash();
            if (Owner.Player != null) await CardPileCmd.Draw(choiceContext, Amount, Owner.Player);
        }
    }
}
