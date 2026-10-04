using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;

namespace ThePrismatic2.ThePrismatic2Code.Relics;

public sealed class AlarmClock: ThePrismatic2Relic
{
    public override RelicRarity Rarity => RelicRarity.Shop;
    public override string PackedIconPath => "ThePrismatic2/images/relics/AlarmClock.png";
    protected override string PackedIconOutlinePath => "ThePrismatic2/images/relics/AlarmClock_Outline.png";
    protected override string BigIconPath => "ThePrismatic2/images/relics/big/AlarmClock.png";

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Innate));

    public override decimal ModifyHandDraw(Player player, decimal count)
    {
        if (player != Owner || Owner.PlayerCombatState is { TurnNumber: > 1 })
        {
            return count;
        }
        int innateCards = Owner.Deck.Cards.Count(card => card.Keywords.Contains(CardKeyword.Innate));
        return count + innateCards;
    }
}