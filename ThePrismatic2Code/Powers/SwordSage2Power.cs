using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class SwordSage2Power : ThePrismatic2Power
{
    private readonly ReplayGrant _replay = new();

    public override string CustomPackedIconPath => "res://.godot/imported/sword_sage_power.png-42fb345bfc32268a2e3c92145734a827.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/sword_sage_power.png-42fb345bfc32268a2e3c92145734a827.s3tc.ctex";

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        HoverTipFactory.FromKeyword(Extensions.Keywords.Costly),
        HoverTipFactory.Static(StaticHoverTip.ReplayStatic)
    ]);

    private static bool IsCostly(CardModel card)
    {
        return card.EnergyCost.GetWithModifiers(CostModifiers.All) + Math.Max(0, card.CurrentStarCost) >= 2;
    }

    private void Sync()
    {
        _replay.Sync(Owner.Player?.PlayerCombatState?.AllCards ?? Array.Empty<CardModel>(), IsCostly, Amount);
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
