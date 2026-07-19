using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class HelixDrill2() : ThePrismatic2Card(0, 
    CardType.Attack, CardRarity.Rare, 
    TargetType.AnyEnemy)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<DefectCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/helix_drill.png-003a037e58198943fa90d7c99290149f.ctex";
    public override string PortraitPath => "res://.godot/imported/helix_drill.png-003a037e58198943fa90d7c99290149f.ctex";

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new DamageVar(3m, ValueProp.Move),
        new CalculationBaseVar(0m),
        new CalculationExtraVar(1m),
        new CalculatedVar("CalculatedHits").WithMultiplier(delegate(CardModel card, Creature? _)
        {
            if (card.Pile == null)
            {
                return 0m;
            }
            int num = (from e in CombatManager.Instance.History.Entries.OfType<EnergySpentEntry>()
                where e.HappenedThisTurn(card.CombatState) && e.Actor.Player == card.Owner
                select e).Sum(c => c.Amount) + (from e in CombatManager.Instance.History.Entries.OfType<StarsModifiedEntry>()
                where e.HappenedThisTurn(card.CombatState) && e.Amount < 0 && e.Actor.Player == card.Owner
                select e).Sum(c => -c.Amount);
            if (card.Pile.Type == PileType.Play)
            {
                num -= card.EnergyCost.GetWithModifiers(CostModifiers.All);
                num -= card.LastStarsSpent;
            }
            return num;
        })
    ]);

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(EnergyHoverTip);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).WithHitCount((int)((CalculatedVar)DynamicVars["CalculatedHits"]).Calculate(cardPlay.Target)).FromCard(this)
            .Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1m);
    }
}