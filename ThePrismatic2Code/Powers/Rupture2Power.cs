using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Cards;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class Rupture2Power : ThePrismatic2Power
{
    
    public override string CustomPackedIconPath => "res://.godot/imported/rupture_power.png-2621f2e97b0d2ddd98fe157942ab88e4.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/rupture_power.png-2621f2e97b0d2ddd98fe157942ab88e4.s3tc.ctex";
    
    private class Data
    {
        public readonly Dictionary<CardModel, int> playedCards = new Dictionary<CardModel, int>();
    }

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<StrengthPower>());

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
        GetInternalData<Data>().playedCards.Add(cardPlay.Card, 0);
        if (cardPlay.Card.Type == CardType.Power)
        {
            PowerCmd.Apply<StrengthPower>(Owner, Amount, Owner, null);
        }
        return Task.CompletedTask;
    }
    
    public override Task BeforeDamageReceived(PlayerChoiceContext choiceContext, Creature target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target == Owner.Player.Osty && CombatState.CurrentSide == Owner.Side)
        {
            PowerCmd.Apply<StrengthPower>(Owner, Amount, Owner, null);
        }
        return Task.CompletedTask;
    }

    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target == Owner && result.UnblockedDamage > 0 && CombatState.CurrentSide == Owner.Side)
        {
            if (cardSource == null || !GetInternalData<Data>().playedCards.ContainsKey(cardSource))
            {
                await PowerCmd.Apply<StrengthPower>(Owner, Amount, Owner, null);
            }
            else
            {
                GetInternalData<Data>().playedCards[cardSource] += Amount;
            }
        }
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature == Owner && GetInternalData<Data>().playedCards.Remove(cardPlay.Card, out var value))
        {
            await PowerCmd.Apply<StrengthPower>(Owner, value, Owner, null);
        }
    }
}
