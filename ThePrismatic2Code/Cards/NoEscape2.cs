using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using ThePrismatic2.ThePrismatic2Code.Character;
using ThePrismatic2.ThePrismatic2Code.Powers;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class NoEscape2() : ThePrismatic2Card(1, 
    CardType.Skill, CardRarity.Uncommon, 
    TargetType.AnyEnemy)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<NecrobinderCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/no_escape.png-5188a65b4cf10cbe66c32b73de491409.ctex";
    public override string PortraitPath => "res://.godot/imported/no_escape.png-5188a65b4cf10cbe66c32b73de491409.ctex";

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new DynamicVar("DebuffThreshold", 10m),
        new DynamicVar("BaseValue", 10m),
        new DynamicVar("ExtraValue", 5m),
        new CalculationBaseVar(0m),
        new CalculationExtraVar(1m),
        new CalculatedVar("CalculatedDebuffs").WithMultiplier(delegate(CardModel card, Creature? target)
        {
            decimal calculationBase = card.IsUpgraded ? 15 : 10;
            int num = target?.Powers.Where(power => power is { Type: PowerType.Debuff, Applier.IsPlayer: true }).Sum(power => power.Amount) ?? 0;
            decimal baseValue = card.DynamicVars["DebuffThreshold"].BaseValue;
            decimal calculatedDoom = Math.Floor(num / baseValue);
            if (target?.HasPower<ExposedPower>() ?? false)
            {
                calculationBase *= target.HasPower<Debilitate2Power>() ? 2m : 1.5m;
                calculatedDoom *= target.HasPower<Debilitate2Power>() ? 2m : 1.5m;
            }
            if (card.Owner.HasPower<Accelerant2Power>())
            {
                calculationBase *= 1 + 0.01m*card.Owner.Creature.GetPowerAmount<Accelerant2Power>();
                calculatedDoom *= 1 + 0.01m*card.Owner.Creature.GetPowerAmount<Accelerant2Power>();
            }
            return calculationBase + 5 * calculatedDoom;
        })
    ]);

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<DoomPower>());

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        int num = cardPlay.Target.Powers.Where(power => power is { Type: PowerType.Debuff, Applier.IsPlayer: true }).Sum(power => power.Amount);
        decimal calculatedDoom = Math.Floor(num / DynamicVars["DebuffThreshold"].BaseValue);
        decimal doomAmount = DynamicVars["BaseValue"].BaseValue + calculatedDoom*DynamicVars["ExtraValue"].BaseValue;
        await PowerCmd.Apply<DoomPower>(choiceContext, cardPlay.Target, doomAmount, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["BaseValue"].UpgradeValueBy(5m);
    }
}