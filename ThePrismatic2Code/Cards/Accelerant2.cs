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
using ThePrismatic2.ThePrismatic2Code.Powers;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Accelerant2() : ThePrismatic2Card(1, 
    CardType.Power, CardRarity.Rare, 
    TargetType.Self)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<SilentCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/accelerant.png-880614f6ed1cd4d533608da0e80ba9de.ctex";
    public override string PortraitPath => "res://.godot/imported/accelerant.png-880614f6ed1cd4d533608da0e80ba9de.ctex";

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        HoverTipFactory.FromPower<PoisonPower>(),
        HoverTipFactory.FromPower<DoomPower>()
        ]);

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Accelerant", 1m));

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<AccelerantPower>(choiceContext, Owner.Creature, DynamicVars["Accelerant"].BaseValue, Owner.Creature, this);
        decimal accelerant2Amount = cardPlay.Target != null && cardPlay.Target.HasPower<Accelerant2Power>() ? DynamicVars["Accelerant"].BaseValue+1 : DynamicVars["Accelerant"].BaseValue;
        await PowerCmd.Apply<Accelerant2Power>(choiceContext, Owner.Creature, accelerant2Amount, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Accelerant"].UpgradeValueBy(1m);
        DynamicVars["Doom"].UpgradeValueBy(50m);
    }
}