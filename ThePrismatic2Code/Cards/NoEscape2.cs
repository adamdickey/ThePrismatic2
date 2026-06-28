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
        new CalculationBaseVar(10m),
        new CalculationExtraVar(5m),
        new CalculatedVar("CalculatedDebuffs").WithMultiplier(delegate(CardModel card, Creature? target)
        {
            int num = target?.Powers.Where(power => power.Type == PowerType.Debuff).Sum(power => power.Amount) ?? 0;
            decimal baseValue = card.DynamicVars["DebuffThreshold"].BaseValue;
            return Math.Floor(num / baseValue);
        })
    ]);

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<DoomPower>());

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await PowerCmd.Apply<DoomPower>(choiceContext, cardPlay.Target, ((CalculatedVar)DynamicVars["CalculatedDebuffs"]).Calculate(cardPlay.Target), Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.CalculationBase.UpgradeValueBy(5m);
    }
}