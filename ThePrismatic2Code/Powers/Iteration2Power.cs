using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class Iteration2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/iteration_power.png-6949520fb4771db5fabaf425ca9c609c.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/iteration_power.png-6949520fb4771db5fabaf425ca9c609c.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if (card.Owner.Creature == Owner && card.Type == CardType.Status)
        {
            int num = CombatManager.Instance.History.Entries.OfType<CardDrawnEntry>().Count(e => e.HappenedThisTurn(CombatState) && e.Actor == Owner && e.Card.Type == CardType.Status) + CombatManager.Instance.History.Entries.OfType<CardGeneratedEntry>().Count(e => e.HappenedThisTurn(CombatState));
            if (num <= 1)
            {
                Flash();
                if (Owner.Player != null) await CardPileCmd.Draw(choiceContext, Amount, Owner.Player);
            }
        }
    }
    public override async Task AfterCardGeneratedForCombat(CardModel card, Player? creator)
    {
        if (card.Owner.Creature == Owner && creator == Owner.Player)
        {
            int num = CombatManager.Instance.History.Entries.OfType<CardDrawnEntry>().Count(e => e.HappenedThisTurn(CombatState) && e.Actor == Owner && e.Card.Type == CardType.Status) + CombatManager.Instance.History.Entries.OfType<CardGeneratedEntry>().Count(e => e.HappenedThisTurn(CombatState));
            if (num <= 1)
            {
                Flash();
                if (Owner.Player != null) await CardPileCmd.Draw(new ThrowingPlayerChoiceContext(), Amount, Owner.Player);
            }
        }
    }
}
