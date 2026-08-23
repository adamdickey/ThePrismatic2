using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Squeeze2() : ThePrismatic2Card(3, 
    CardType.Attack, CardRarity.Rare, 
    TargetType.AnyEnemy)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<NecrobinderCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/squeeze.png-b8c7515cbade9c93aa1b0bea50d6db1e.ctex";
    public override string PortraitPath => "res://.godot/imported/squeeze.png-b8c7515cbade9c93aa1b0bea50d6db1e.ctex";

    protected override HashSet<CardTag> CanonicalTags => [ CardTag.OstyAttack ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(Extensions.Keywords.Costly));

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new SummonVar(5m),
        new CalculationBaseVar(15m),
        new ExtraDamageVar(5m).FromOsty(),
        new CalculatedDamageVar(ValueProp.Move).FromOsty().WithMultiplier((card, _) =>
        {
            if (card.Owner.PlayerCombatState != null)
                return card.Owner.PlayerCombatState.AllCards.Count(c => (c.Tags.Contains(CardTag.OstyAttack) || c.EnergyCost.GetWithModifiers(CostModifiers.All) + Math.Max(0, c.CurrentStarCost) >= 2) && c != card);
            return 0m;
        })
    ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await OstyCmd.Summon(choiceContext, Owner, DynamicVars.Summon.BaseValue, this);
        if (!Osty.CheckMissingWithAnim(Owner) && Owner.Osty != null)
        {
            await DamageCmd.Attack(DynamicVars.CalculatedDamage).FromOsty(Owner.Osty, this).Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
                .Execute(choiceContext);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Summon.UpgradeValueBy(1m);
        DynamicVars.CalculationBase.UpgradeValueBy(5m);
        DynamicVars.ExtraDamage.UpgradeValueBy(1m);
    }
}