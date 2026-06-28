using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class FlakCannon2() : ThePrismatic2Card(2, 
    CardType.Attack, CardRarity.Rare, 
    TargetType.RandomEnemy)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<DefectCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/flak_cannon.png-82166775b4819c97d156e25fb5d9c2de.ctex";
    public override string PortraitPath => "res://.godot/imported/flak_cannon.png-82166775b4819c97d156e25fb5d9c2de.ctex";

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new DamageVar(8m, ValueProp.Move),
        new CalculationBaseVar(0m),
        new CalculationExtraVar(1m),
        new CalculatedVar("CalculatedHits").WithMultiplier((card, _) => (GetStatuses(card.Owner) ?? throw new InvalidOperationException()).Count())
    ]);

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Exhaust));

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        List<CardModel> list = (GetStatuses(Owner) ?? throw new InvalidOperationException()).ToList();
        int statusCount = (int)((CalculatedVar)DynamicVars["CalculatedHits"]).Calculate(cardPlay.Target);
        foreach (CardModel item in list)
        {
            await CardCmd.Exhaust(choiceContext, item);
        }

        if (CombatState != null)
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).WithHitCount(statusCount).FromCard(this)
                .TargetingRandomOpponents(CombatState)
                .WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
                .Execute(choiceContext);
    }

    private static IEnumerable<CardModel>? GetStatuses(Player owner)
    {
        return owner.PlayerCombatState?.AllCards.Where(c => (c.Type == CardType.Status || c.VisualCardPool.IsColorless) && c.Pile != null && c.Pile.Type != PileType.Exhaust);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
    }
}