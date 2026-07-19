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
    CardType.Power, CardRarity.Uncommon, 
    TargetType.Self)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<SilentCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/accelerant.png-880614f6ed1cd4d533608da0e80ba9de.ctex";
    public override string PortraitPath => "res://.godot/imported/accelerant.png-880614f6ed1cd4d533608da0e80ba9de.ctex";

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        HoverTipFactory.FromPower<PoisonPower>(),
        HoverTipFactory.FromPower<DoomPower>()
        ]);

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new DynamicVar("Poison", 1m),
        new DynamicVar("Doom", 50m)
        ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<AccelerantPower>(choiceContext, Owner.Creature, DynamicVars["Poison"].BaseValue, Owner.Creature, this);
        await PowerCmd.Apply<Accelerant2Power>(choiceContext, Owner.Creature, DynamicVars["Doom"].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Poison"].UpgradeValueBy(1m);
        DynamicVars["Doom"].UpgradeValueBy(50m);
    }
}