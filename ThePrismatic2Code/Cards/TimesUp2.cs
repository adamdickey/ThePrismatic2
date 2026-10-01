using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
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
public class TimesUp2() : ThePrismatic2Card(2,
    CardType.Attack, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<NecrobinderCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/times_up.png-714b702fd2a54f37beb2080e183f0173.ctex";
    public override string PortraitPath => "res://.godot/imported/times_up.png-714b702fd2a54f37beb2080e183f0173.ctex";

    protected override bool ShouldGlowGoldInternal => CombatState?.HittableEnemies.Any(e => e.Powers.Count(power => power.Type == PowerType.Debuff) >= 4) ?? false;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<DoomPower>());

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new PowerVar<DoomPower>(20m),
        new DynamicVar("BigDoom", 100m)
    ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        int debuffCount = cardPlay.Target.Powers.Count(power => power.Type == PowerType.Debuff);
        decimal doom = debuffCount >= 4 ? DynamicVars["BigDoom"].BaseValue : DynamicVars.Doom.BaseValue;
        await PowerCmd.Apply<DoomPower>(choiceContext, cardPlay.Target, doom, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }
}
