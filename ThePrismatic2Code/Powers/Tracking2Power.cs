using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class Tracking2Power : ThePrismatic2Power
{
    private readonly ReplayGrant _replay = new();

    public override string CustomPackedIconPath => "res://.godot/imported/tracking_power.png-c92782dc2d561036e85808d62312bb58.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/tracking_power.png-c92782dc2d561036e85808d62312bb58.s3tc.ctex";

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.ReplayStatic));

    private bool Active => CombatState?.HittableEnemies.Any(e => e.Powers.Count(power => power.Type == PowerType.Debuff) >= 2) ?? false;

    private void Sync()
    {
        bool active = Active;
        _replay.Sync(Owner.Player?.PlayerCombatState?.AllCards ?? Array.Empty<CardModel>(),
            card => active && card.Type == CardType.Attack, Amount);
    }

    public override Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        Sync();
        return Task.CompletedTask;
    }

    public override Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        Sync();
        return Task.CompletedTask;
    }

    public override Task AfterCardEnteredCombat(CardModel card)
    {
        Sync();
        return Task.CompletedTask;
    }

    public override Task AfterCardGeneratedForCombat(CardModel card, Player? creator)
    {
        Sync();
        return Task.CompletedTask;
    }

    public override Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        Sync();
        return Task.CompletedTask;
    }

    public override Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        Sync();
        return Task.CompletedTask;
    }

    public override Task AfterRemoved(Creature? creature)
    {
        _replay.Clear();
        return Task.CompletedTask;
    }

    public override Task AfterCombatEnd(CombatRoom room)
    {
        _replay.Clear();
        return Task.CompletedTask;
    }
}
