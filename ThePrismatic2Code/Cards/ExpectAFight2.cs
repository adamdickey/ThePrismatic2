using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class ExpectAFight2() : ThePrismatic2Card(2, 
    CardType.Skill, CardRarity.Uncommon, 
    TargetType.Self)
{
    public override string CustomPortraitPath => "res://.godot/imported/expect_a_fight.png-5165f60f261f121fa2e79a05ae4c1896.ctex";
    public override string PortraitPath => "res://.godot/imported/expect_a_fight.png-5165f60f261f121fa2e79a05ae4c1896.ctex";
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => new _003C_003Ez__ReadOnlySingleElementList<CardKeyword>(Extensions.Keywords.Costly);
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        EnergyHoverTip,
        HoverTipFactory.FromKeyword(Extensions.Keywords.Costly)
    ]);

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new EnergyVar(0),
        new CalculationBaseVar(0m),
        new CalculationExtraVar(1m),
        new CalculatedVar("CalculatedEnergy").WithMultiplier((card, _) => 
            PileType.Hand.GetPile(card.Owner).Cards.Count(c => c != card &&
                                                                 (c.Type == CardType.Attack || c.EnergyCost.GetResolved() + Math.Max(0, c.CurrentStarCost) >= 2)))
    ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await PlayerCmd.GainEnergy(((CalculatedVar)DynamicVars["CalculatedEnergy"]).Calculate(cardPlay.Target), Owner);
        await PowerCmd.Apply<NoEnergyGainPower>(Owner.Creature, 1m, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}