using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;

namespace ThePrismatic2.ThePrismatic2Code.Relics;

public sealed class AlarmClock: ThePrismatic2Relic
{
    public override RelicRarity Rarity => RelicRarity.Shop;
    //public override string PackedIconPath => "res://images/atlases/relic_atlas.sprites/burning_blood.tres";
    //protected override string PackedIconOutlinePath => "res://images/atlases/relic_outline_atlas.sprites/burning_blood.tres";
    //protected override string BigIconPath => "res://images/relics/burning_blood.png";

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