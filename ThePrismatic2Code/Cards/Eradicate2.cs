using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Eradicate2() : ThePrismatic2Card(0, 
    CardType.Attack, CardRarity.Rare, 
    TargetType.AnyEnemy)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<NecrobinderCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/eradicate.png-455875ed65fe1196e35052806aefb87d.ctex";
    public override string PortraitPath => "res://.godot/imported/eradicate.png-455875ed65fe1196e35052806aefb87d.ctex";
    
    protected override bool HasEnergyCostX => true;

    public override IEnumerable<CardKeyword> CanonicalKeywords => new _003C_003Ez__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Retain);

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new CalculationBaseVar(9m),
        new ExtraDamageVar(2m),
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier((card, _) => PileType.Hand.GetPile(card.Owner).Cards.Count(c => c.ShouldRetainThisTurn) - 1)
        ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        decimal damage = DynamicVars.CalculatedDamage.BaseValue + DynamicVars.ExtraDamage.BaseValue * PileType.Hand.GetPile(Owner).Cards.Count(c => c.ShouldRetainThisTurn);
        await DamageCmd.Attack(damage).WithHitCount(ResolveEnergyXValue()).FromCard(this)
            .Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.CalculationBase.UpgradeValueBy(2m);
        DynamicVars.ExtraDamage.UpgradeValueBy(1m);
    }
}