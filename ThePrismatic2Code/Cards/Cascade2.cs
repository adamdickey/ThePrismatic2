using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Cascade2() : ThePrismatic2Card(-1, 
    CardType.Skill, CardRarity.Rare, 
    TargetType.Self)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<IroncladCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/cascade.png-22f4023b86c70080ddd48e144bd8cd53.ctex";
    public override string PortraitPath => "res://.godot/imported/cascade.png-22f4023b86c70080ddd48e144bd8cd53.ctex";

    protected override bool HasEnergyCostX => true;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        IEnumerable<CardModel> enumerable = PileType.Hand.GetPile(Owner).Cards.ToList();
        await CardCmd.Discard(choiceContext, enumerable);
        int num = ResolveEnergyXValue();
        if (IsUpgraded)
        {
            num++;
        }
        await CardPileCmd.AutoPlayFromDrawPile(choiceContext, Owner, num, CardPilePosition.Top, forceExhaust: false);
    }
}