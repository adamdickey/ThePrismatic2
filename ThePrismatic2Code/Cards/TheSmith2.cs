using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class TheSmith2() : ThePrismatic2Card(1, 
    CardType.Skill, CardRarity.Rare, 
    TargetType.Self)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<RegentCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/the_smith.png-a61b6999e97bf60de2237dfb87a703a5.ctex";
    public override string PortraitPath => "res://.godot/imported/the_smith.png-a61b6999e97bf60de2237dfb87a703a5.ctex";
    
    public override int CanonicalStarCost => 3;
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => new _003C_003Ez__ReadOnlySingleElementList<CardKeyword>(Extensions.Keywords.Starbound);

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new ForgeVar(25));

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        ..HoverTipFactory.FromForge(),
        HoverTipFactory.FromKeyword(Extensions.Keywords.Costly)
        ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await ForgeCmd.Forge(DynamicVars.Forge.IntValue, Owner, this);
        foreach (CardModel card in PileType.Hand.GetPile(Owner).Cards)
        {
            if (card.EnergyCost.GetWithModifiers(CostModifiers.All) + Math.Max(0, card.GetStarCostWithModifiers()) >= 2)
            {
                CardCmd.Upgrade(card);
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Forge.UpgradeValueBy(10m);
    }
}