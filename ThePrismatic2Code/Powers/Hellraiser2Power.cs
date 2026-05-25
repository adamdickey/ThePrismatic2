using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class Hellraiser2Power : ThePrismatic2Power
{
    
    public override string CustomPackedIconPath => "res://.godot/imported/hellraiser_power.png-c5bdab4c22ddca8ac5d345665d55d1b4.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/hellraiser_power.png-c5bdab4c22ddca8ac5d345665d55d1b4.s3tc.ctex";
    
    private HashSet<CardModel>? _autoplayingCards;

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Single;

    private HashSet<CardModel> AutoplayingCards
    {
        get
        {
            AssertMutable();
            if (_autoplayingCards == null)
            {
                _autoplayingCards = new HashSet<CardModel>();
            }
            return _autoplayingCards;
        }
    }

    public override async Task AfterCardDrawnEarly(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if (card.Owner.Creature == Owner && (card.Tags.Contains(CardTag.Strike) || (card.Keywords.Contains(Extensions.Keywords.Cunning) && card.Type == CardType.Attack)) && !Owner.CombatState.HittableEnemies.All((Creature c) => c.ShowsInfiniteHp))
        {
            AutoplayingCards.Add(card);
            await CardCmd.AutoPlay(choiceContext, card, null);
            AutoplayingCards.Remove(card);
        }
    }

    public override Task BeforeAttack(AttackCommand command)
    {
        if (!AutoplayingCards.Contains(command.ModelSource))
        {
            return Task.CompletedTask;
        }

        if (AutoplayingCards.Any(card => card.Tags.Contains(CardTag.OstyAttack)))
        {
            return Task.CompletedTask;
        }
        command.WithHitFx("vfx/hellraiser_attack_vfx", command.HitSfx, command.TmpHitSfx).WithAttackerAnim("Cast", command.Attacker.Player.Character.CastAnimDelay).SpawningHitVfxOnEachCreature()
            .WithHitVfxSpawnedAtBase();
        return Task.CompletedTask;
    }
}
