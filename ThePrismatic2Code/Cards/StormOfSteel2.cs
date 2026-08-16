using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class StormOfSteel2() : ThePrismatic2Card(1, 
    CardType.Skill, CardRarity.Rare, 
    TargetType.Self)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<SilentCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/storm_of_steel.png-bd0bb51e06b1ba275d6d6366a9960d26.ctex";
    public override string PortraitPath => "res://.godot/imported/storm_of_steel.png-bd0bb51e06b1ba275d6d6366a9960d26.ctex";
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<Shiv>(IsUpgraded));

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        IEnumerable<CardModel> enumerable = PileType.Hand.GetPile(Owner).Cards.ToList();
        int handSize = enumerable.Count();
        await CardCmd.Discard(choiceContext, enumerable);
        await Cmd.CustomScaledWait(0f, 0.25f);
        if (CombatState != null)
        {
            IEnumerable<CardModel> enumerable2 = await Shiv.CreateInHand(Owner, handSize, CombatState);
            if (!IsUpgraded)
            {
                return;
            }
            foreach (CardModel item in enumerable2)
            {
                CardCmd.Upgrade(item);
            }
        }
    }
}