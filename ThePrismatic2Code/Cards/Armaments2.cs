using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Orbs;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Armaments2() : ThePrismatic2Card(1,
    CardType.Skill, CardRarity.Common,
    TargetType.Self)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<IroncladCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/armaments.png-48d04dc8b54801754079bd64a065281d.ctex";
    public override string PortraitPath => "res://.godot/imported/armaments.png-48d04dc8b54801754079bd64a065281d.ctex";
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        HoverTipFactory.Static(StaticHoverTip.Channeling),
        HoverTipFactory.FromOrb<FrostOrb>()
    ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await OrbCmd.Channel<FrostOrb>(choiceContext, Owner);
        if (IsUpgraded)
        {
            foreach (CardModel item in PileType.Hand.GetPile(Owner).Cards.Where(c => c.IsUpgradable))
            {
                CardCmd.Upgrade(item);
            }
            return;
        }
        CardModel? cardModel = await CardSelectCmd.FromHandForUpgrade(choiceContext, Owner, this);
        if (cardModel != null)
        {
            CardCmd.Upgrade(cardModel);
        }
    }
}