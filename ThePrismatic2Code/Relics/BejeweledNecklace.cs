using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;
using ThePrismatic2.ThePrismatic2Code.Cards;

namespace ThePrismatic2.ThePrismatic2Code.Relics;

public sealed class BejeweledNecklace: ThePrismatic2Relic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    //public override string PackedIconPath => "res://images/atlases/relic_atlas.sprites/burning_blood.tres";
    //protected override string PackedIconOutlinePath => "res://images/atlases/relic_outline_atlas.sprites/burning_blood.tres";
    //protected override string BigIconPath => "res://images/relics/burning_blood.png";

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        HoverTipFactory.FromCard<Hang2>(upgrade: true),
        ..HoverTipFactory.FromEnchantment<Glam>()
    ]);
    
    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new CardsVar(1));

    public override async Task AfterObtained()
    {
        List<CardPileAddResult> hangs = new List<CardPileAddResult>();
        for (int i = 0; i < DynamicVars.Cards.IntValue; i++)
        {
            CardModel card2 = Owner.RunState.CreateCard(ModelDb.Card<Hang2>(), Owner);
            CardCmd.Upgrade(card2);
            CardCmd.Enchant<Glam>(card2, 1m);
            hangs.Add(await CardPileCmd.Add(card2, PileType.Deck));
        }
        CardCmd.PreviewCardPileAdd(hangs, 2f);
        await Cmd.Wait(0.75f);
    }
}