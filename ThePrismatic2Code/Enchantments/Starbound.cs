using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace ThePrismatic2.ThePrismatic2Code.Enchantments;

public sealed class Starbound : CustomEnchantmentModel
{
    protected override string CustomIconPath => "res://.godot/imported/inky.png-27b90d1ea6c2aab29a64059ea46c014c.ctex";

    public override bool ShowAmount => false;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(Extensions.Keywords.Starbound));

    public override bool TryModifyKeywordsInCombat(CardModel card, ISet<CardKeyword> keywords)
    {
        if (card.Enchantment is not Starbound)
        {
            return false;
        }
        return keywords.Add(Extensions.Keywords.Starbound);
    }
}
