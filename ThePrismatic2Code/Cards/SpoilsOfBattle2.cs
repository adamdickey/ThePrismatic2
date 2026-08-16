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
public class SpoilsOfBattle2() : ThePrismatic2Card(1, 
    CardType.Skill, CardRarity.Common, 
    TargetType.Self)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<RegentCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/spoils_of_battle.png-b5069ec2221a0488c22d8826137efc81.ctex";
    public override string PortraitPath => "res://.godot/imported/spoils_of_battle.png-b5069ec2221a0488c22d8826137efc81.ctex";
    
    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new ForgeVar(6),
        new CardsVar(1)
    ]);

    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromForge();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await ForgeCmd.Forge(DynamicVars.Forge.IntValue, Owner, this);
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
        IEnumerable<CardModel> colorlessCards = PileType.Hand.GetPile(Owner).Cards.Where(c => c.VisualCardPool.IsColorless);
        foreach (CardModel card in colorlessCards)
        {
            CardCmd.Upgrade(card);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1m);
    }
}