using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class TheSealedThrone2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/the_sealed_throne_power.png-18fc8f50add9d4ba2cb6765101f0dfb8.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/the_sealed_throne_power.png-18fc8f50add9d4ba2cb6765101f0dfb8.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(Extensions.Keywords.Starbound));
    public override bool TryModifyKeywordsInCombat(CardModel card, ISet<CardKeyword> keywords)
    {
        if (card.Owner != Owner.Player)
        {
            return false;
        }
        if (card.Keywords.Contains(Extensions.Keywords.StarboundThisTurn))
        {
            card.RemoveKeyword(Extensions.Keywords.StarboundThisTurn);
        }
        return keywords.Add(Extensions.Keywords.Starbound);
    }
    
    public override async Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner == Owner.Player)
        {
            int cardsPlayedThisTurn = CombatManager.Instance.History.CardPlaysStarted.Count(e => e.Actor == Owner && e.CardPlay.IsFirstInSeries && e.HappenedThisTurn(CombatState));
            if (cardsPlayedThisTurn < Amount)
            {
                Flash();
                await PlayerCmd.GainStars(Amount, Owner.Player);
            }
        }
    }
}
