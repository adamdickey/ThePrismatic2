using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using ThePrismatic2.ThePrismatic2Code.Cards;

namespace ThePrismatic2.ThePrismatic2Code.Relics;

public sealed class FatefulHourglass: ThePrismatic2Relic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    //public override string PackedIconPath => "res://images/atlases/relic_atlas.sprites/burning_blood.tres";
    //protected override string PackedIconOutlinePath => "res://images/atlases/relic_outline_atlas.sprites/burning_blood.tres";
    //protected override string BigIconPath => "res://images/relics/burning_blood.png";

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<MakeItSo2>(upgrade: true));
    
    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new CardsVar(2));

    public override async Task AfterObtained()
    {
        List<CardPileAddResult> makeItSos = new List<CardPileAddResult>();
        for (int i = 0; i < DynamicVars.Cards.IntValue; i++)
        {
            CardModel card2 = Owner.RunState.CreateCard(ModelDb.Card<MakeItSo2>(), Owner);
            CardCmd.Upgrade(card2);
            makeItSos.Add(await CardPileCmd.Add(card2, PileType.Deck));
        }
        CardCmd.PreviewCardPileAdd(makeItSos, 2f);
        await Cmd.Wait(0.75f);
    }
}