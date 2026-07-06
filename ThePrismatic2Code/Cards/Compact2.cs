using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Compact2() : ThePrismatic2Card(1, 
    CardType.Skill, CardRarity.Uncommon, 
    TargetType.Self)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<DefectCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/compact.png-e6e5083679501001eae69d7356fdd5d2.ctex";
    public override string PortraitPath => "res://.godot/imported/compact.png-e6e5083679501001eae69d7356fdd5d2.ctex";
    
    public override bool GainsBlock => true;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        HoverTipFactory.FromCard<Fuel>(IsUpgraded),
        HoverTipFactory.Static(StaticHoverTip.Transform),
        HoverTipFactory.ForEnergy(this)
    ]);

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new BlockVar(6m, ValueProp.Move));

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        List<CardModel> list = PileType.Hand.GetPile(Owner).Cards.Where(c => c.IsTransformable && (c.Type == CardType.Status || c.VisualCardPool.IsColorless)).ToList();
        foreach (CardModel item in list)
        {
            CardModel? cardModel = CombatState?.CreateCard<Fuel>(Owner);
            if (IsUpgraded && cardModel != null)
            {
                CardCmd.Upgrade(cardModel);
            }
            if (cardModel != null)
            {
                await CardCmd.Transform(item, cardModel);
            }
            
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(1m);
    }
}