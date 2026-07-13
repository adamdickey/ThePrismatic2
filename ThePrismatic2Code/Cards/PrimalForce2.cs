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
public class PrimalForce2() : ThePrismatic2Card(0, 
    CardType.Skill, CardRarity.Rare, 
    TargetType.Self)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<IroncladCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/primal_force.png-f5f0b5b661403bd76cfef60893134b8d.ctex";
    public override string PortraitPath => "res://.godot/imported/primal_force.png-f5f0b5b661403bd76cfef60893134b8d.ctex";
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        HoverTipFactory.FromKeyword(CardKeyword.Exhaust),
        HoverTipFactory.FromCard<GiantRock>(IsUpgraded)
        ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        List<CardModel> list = PileType.Hand.GetPile(Owner).Cards.Where(c => c.IsTransformable && c.Type == CardType.Attack).ToList();
        foreach (CardModel item in list)
        {
            await CardCmd.Exhaust(choiceContext, item);
            CardModel? cardModel = CombatState?.CreateCard<GiantRock>(Owner);
            if (cardModel == null) continue;
            if (IsUpgraded)
            {
                CardCmd.Upgrade(cardModel);
            }
            await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Hand, Owner);
        }
    }
}