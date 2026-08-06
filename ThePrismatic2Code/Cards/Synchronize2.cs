using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Synchronize2() : ThePrismatic2Card(1, 
    CardType.Skill, CardRarity.Uncommon, 
    TargetType.Self)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<DefectCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/synchronize.png-24826618afcfde4513d1246835349d1b.ctex";
    public override string PortraitPath => "res://.godot/imported/synchronize.png-24826618afcfde4513d1246835349d1b.ctex";

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<FocusPower>());

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new DynamicVar("Focus", 2m),
        new DynamicVar("Forge", 5m),
        new CalculationBaseVar(0m),
        new CalculationExtraVar(1m),
        new CalculatedVar("CalculatedFocus").WithMultiplier((card, _) => (from orb in card.Owner.PlayerCombatState?.OrbQueue.Orbs
            group orb by orb.Id).Count())
    ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<SynchronizePower>(choiceContext, Owner.Creature, ((CalculatedVar)DynamicVars["CalculatedFocus"]).Calculate(cardPlay.Target) * DynamicVars["Focus"].BaseValue, Owner.Creature, this);
        await ForgeCmd.Forge(((CalculatedVar)DynamicVars["CalculatedFocus"]).Calculate(cardPlay.Target) * DynamicVars["Forge"].BaseValue, Owner, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Forge.UpgradeValueBy(3m);
    }
}