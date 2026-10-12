using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace ThePrismatic2.ThePrismatic2Code.Enchantments;

public sealed class Starbound : CustomEnchantmentModel
{
    protected override string CustomIconPath => "res://.godot/imported/energy_star.png-c2b438593c00931cfb9a396d3aafe9a9.ctex";

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        HoverTipFactory.FromKeyword(Extensions.Keywords.Starbound)
    ]);
    
    public override bool CanEnchant(CardModel card)
    {
        return !card.Keywords.Contains(Extensions.Keywords.Starbound);
    }

    protected override void OnEnchant()
    {
        Card.AddKeyword(Extensions.Keywords.Starbound);
    }
}