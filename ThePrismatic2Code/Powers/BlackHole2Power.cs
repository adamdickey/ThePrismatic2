using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class BlackHole2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/black_hole_power.png-da323199e9ac2c27b67e28c05b936610.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/black_hole_power.png-da323199e9ac2c27b67e28c05b936610.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    private class Data
    {
        public readonly Dictionary<CardModel, int> PlayedCards = new();
    }
    
    protected override object InitInternalData()
    {
        return new Data();
    }
    
    public override async Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature != Owner)
        {
            return;
        }
        if (CombatState.CurrentSide != Owner.Side)
        {
            return;
        }
        GetInternalData<Data>().PlayedCards.Add(cardPlay.Card, 0);
        if (cardPlay.Card.Type == CardType.Power)
        {
            await DealDamageToAllEnemies();
        }
    }
    
    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature == Owner && GetInternalData<Data>().PlayedCards.Remove(cardPlay.Card, out var value))
        {
            if (value > 0)
            {
                Flash();
                await CreatureCmd.Damage(new BlockingPlayerChoiceContext(), CombatState.HittableEnemies, value, ValueProp.Unpowered, Owner, null);
            }
        }
        if (cardPlay.Resources.StarsSpent > 0 && cardPlay.Card.Owner == Owner.Player && cardPlay.IsLastInSeries)
        {
            await DealDamageToAllEnemies();
        }
    }

    public override async Task AfterStarsGained(int amount, Player gainer)
    {
        if (amount > 0 && gainer == Owner.Player)
        {
            await DealDamageToAllEnemies();
        }
    }

    private async Task DealDamageToAllEnemies()
    {
        Flash();
        await CreatureCmd.Damage(new BlockingPlayerChoiceContext(), CombatState.HittableEnemies, Amount, ValueProp.Unpowered, Owner, null);
    }
}
