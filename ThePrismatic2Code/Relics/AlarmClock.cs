using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace ThePrismatic2.ThePrismatic2Code.Relics;

public sealed class AlarmClock: ThePrismatic2Relic
{
    public override RelicRarity Rarity => RelicRarity.Shop;
    public override string PackedIconPath => "ThePrismatic2/images/relics/AlarmClock.png";
    protected override string PackedIconOutlinePath => "ThePrismatic2/images/relics/AlarmClock_Outline.png";
    protected override string BigIconPath => "ThePrismatic2/images/relics/big/AlarmClock.png";

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Innate));

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new CardsVar(2));

    public override decimal ModifyHandDraw(Player player, decimal count)
    {
        if (player != Owner || Owner.PlayerCombatState is { TurnNumber: > 1 })
        {
            return count;
        }
        int innateCards = Owner.Deck.Cards.Count(card => card.Keywords.Contains(CardKeyword.Innate));
        if (innateCards > 0) Flash();
        return count + DynamicVars.Cards.IntValue * innateCards;
    }
}