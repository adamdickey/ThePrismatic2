using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
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

    private const string _calculatedEnergyKey = "CalculatedEnergy";
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => new global::_003C_003Ez__ReadOnlySingleElementList<CardKeyword>(Extensions.Keywords.Costly);

    protected override IEnumerable<DynamicVar> CanonicalVars => new global::_003C_003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[4]
    {
        new EnergyVar(0),
        new CalculationBaseVar(0m),
        new CalculationExtraVar(1m),
        new CalculatedVar("CalculatedEnergy").WithMultiplier((CardModel card, Creature? _) => 
            PileType.Hand.GetPile(card.Owner).Cards.Count((CardModel c) => c != card &&
                (c.Type == CardType.Attack || c.EnergyCost.GetResolved() + Math.Max(0, c.CurrentStarCost) >= 2)))
    });

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new global::_003C_003Ez__ReadOnlySingleElementList<IHoverTip>(base.EnergyHoverTip);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        await PlayerCmd.GainEnergy(((CalculatedVar)base.DynamicVars["CalculatedEnergy"]).Calculate(cardPlay.Target), base.Owner);
        await PowerCmd.Apply<NoEnergyGainPower>(base.Owner.Creature, 1m, base.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}