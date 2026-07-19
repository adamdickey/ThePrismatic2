using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using ThePrismatic2.ThePrismatic2Code.Orbs;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class Genesis2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/genesis_power.png-fbb5fcea8f8c7afebbb2cc171c10c2d4.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/genesis_power.png-fbb5fcea8f8c7afebbb2cc171c10c2d4.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        HoverTipFactory.FromKeyword(Extensions.Keywords.Starbound),
        HoverTipFactory.Static(StaticHoverTip.Channeling),
        HoverTipFactory.FromOrb<SolarOrb>()
    ]);
    
    public override Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (!(power is Genesis2Power))
        {
            return Task.CompletedTask;
        }
        if (power.Owner != Owner)
        {
            return Task.CompletedTask;
        }
        IEnumerable<CardModel> enumerable = Owner.Player?.PlayerCombatState?.AllCards ?? Array.Empty<CardModel>();
        foreach (CardModel card in enumerable)
        {
            if (card.Type != CardType.Power) continue;
            if (card.Keywords.Contains(Extensions.Keywords.StarboundThisTurn))
            {
                card.RemoveKeyword(Extensions.Keywords.StarboundThisTurn);
            }
            if (!card.Keywords.Contains(Extensions.Keywords.Starbound))
            {
                card.AddKeyword(Extensions.Keywords.Starbound);
            }
        }
        return Task.CompletedTask;
    }

    public override Task AfterCardEnteredCombat(CardModel card)
    {
        if (card.Type != CardType.Power) return Task.CompletedTask;
        if (card.Keywords.Contains(Extensions.Keywords.StarboundThisTurn))
        {
            card.RemoveKeyword(Extensions.Keywords.StarboundThisTurn);
        }
        if (!card.Keywords.Contains(Extensions.Keywords.Starbound))
        {
            card.AddKeyword(Extensions.Keywords.Starbound);
        }
        return Task.CompletedTask;
    }

    public override Task AfterRemoved(Creature oldOwner)
    {
        IEnumerable<CardModel> enumerable = oldOwner.Player?.PlayerCombatState?.AllCards ?? Array.Empty<CardModel>();
        foreach (CardModel item in enumerable)
        {
            if (item.Type == CardType.Power && item.Keywords.Contains(Extensions.Keywords.Starbound))
            {
                item.RemoveKeyword(Extensions.Keywords.Starbound);
            }
        }
        return Task.CompletedTask;
    }

    public override async Task AfterEnergyReset(Player player)
    {
        if (player == Owner.Player)
        {
            Flash();
            await PlayerCmd.GainStars(Amount, Owner.Player);
        }
    }
}
