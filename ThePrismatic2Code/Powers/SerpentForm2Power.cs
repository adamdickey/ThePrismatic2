using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class SerpentForm2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/serpent_form_power.png-6fd13dea609ec483ec021ab1c3580aab.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/serpent_form_power.png-6fd13dea609ec483ec021ab1c3580aab.s3tc.ctex";
    
    private class Data
    {
        public readonly Dictionary<CardModel, int> amountsForPlayedCards = new();
    }

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override object InitInternalData()
    {
        return new Data();
    }

    public override Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner.Player)
        {
            return Task.CompletedTask;
        }
        GetInternalData<Data>().amountsForPlayedCards.Add(cardPlay.Card, Amount);
        return Task.CompletedTask;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner == Owner.Player && GetInternalData<Data>().amountsForPlayedCards.Remove(cardPlay.Card, out var damage) && damage > 0)
        {
            await Cmd.CustomScaledWait(0.1f, 0.2f);
            Creature creature = Owner.Player.RunState.Rng.CombatTargets.NextItem(Owner.CombatState.HittableEnemies);
            if (creature != null)
            {
                await PowerCmd.Apply<DoomPower>(creature, damage, Owner, null);
            }
        }
    }
}
