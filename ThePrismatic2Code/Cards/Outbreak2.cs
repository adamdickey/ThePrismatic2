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
public class Outbreak2() : ThePrismatic2Card(3,
    CardType.Skill, CardRarity.Rare,
    TargetType.AllEnemies)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<SilentCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/outbreak.png-68e282f3c4c29d8ecda8fd4edda58fd2.ctex";
    public override string PortraitPath => "res://.godot/imported/outbreak.png-68e282f3c4c29d8ecda8fd4edda58fd2.ctex";

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        HoverTipFactory.FromPower<PoisonPower>(),
        HoverTipFactory.FromPower<DoomPower>(),
        HoverTipFactory.FromPower<WeakPower>(),
        HoverTipFactory.FromPower<VulnerablePower>(),
        HoverTipFactory.FromPower<ExposedPower>()
    ]);

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new PowerVar<PoisonPower>(9m),
        new PowerVar<DoomPower>(20m),
        new PowerVar<WeakPower>(3m),
        new PowerVar<VulnerablePower>(3m),
        new DynamicVar("Exposed", 3m)
    ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        if (CombatState?.HittableEnemies != null)
        {
            await PowerCmd.Apply<PoisonPower>(choiceContext, CombatState.HittableEnemies, DynamicVars.Poison.BaseValue, Owner.Creature, this);
            await PowerCmd.Apply<DoomPower>(choiceContext, CombatState.HittableEnemies, DynamicVars.Doom.BaseValue, Owner.Creature, this);
            await PowerCmd.Apply<WeakPower>(choiceContext, CombatState.HittableEnemies, DynamicVars.Weak.BaseValue, Owner.Creature, this);
            await PowerCmd.Apply<VulnerablePower>(choiceContext, CombatState.HittableEnemies, DynamicVars.Vulnerable.BaseValue, Owner.Creature, this);
            await PowerCmd.Apply<ExposedPower>(choiceContext, CombatState.HittableEnemies, DynamicVars["Exposed"].BaseValue, Owner.Creature, this);
        }
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}
