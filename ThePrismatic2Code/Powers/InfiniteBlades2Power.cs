using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class InfiniteBlades2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/infinite_blades_power.png-7f20242936ccb8302210c3fb3ae623b4.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/infinite_blades_power.png-7f20242936ccb8302210c3fb3ae623b4.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<Shiv>());
    
    private class Data
    {
        public readonly Dictionary<CardModel, int> PlayedCards = new();
    }
    
    protected override object InitInternalData()
    {
        return new Data();
    }
    
    public override Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature != Owner)
        {
            return Task.CompletedTask;
        }
        if (CombatState.CurrentSide != Owner.Side)
        {
            return Task.CompletedTask;
        }
        GetInternalData<Data>().PlayedCards.Add(cardPlay.Card, 0);
        if (cardPlay.Card.Type == CardType.Power)
        {
            Flash();
            if (Owner.Player != null) Shiv.CreateInHand(Owner.Player, Amount, CombatState);
        }
        return Task.CompletedTask;
    }

    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
    {
        if (player == Owner.Player)
        {
            Flash();
            await Shiv.CreateInHand(Owner.Player, Amount, combatState);
        }
    }
    
    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature == Owner && GetInternalData<Data>().PlayedCards.Remove(cardPlay.Card, out var value))
        {
            Flash();
            if (Owner.Player != null) await Shiv.CreateInHand(Owner.Player, value, CombatState);
        }
    }
}
