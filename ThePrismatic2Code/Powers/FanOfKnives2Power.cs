using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class FanOfKnives2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/fan_of_knives_power.png-e83f85683b9a996deb18f8d48017d311.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/fan_of_knives_power.png-e83f85683b9a996deb18f8d48017d311.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Single;

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CardModel? lastPlayedCard = CombatManager.Instance.History.Entries.OfType<CardPlayFinishedEntry>().ElementAtOrDefault(^2)?.CardPlay.Card;
        if (cardPlay is { Card: { Type: CardType.Attack, TargetType: TargetType.AnyEnemy }, Resources.EnergySpent: 0 })
        {
            if (cardPlay.Card != lastPlayedCard || !cardPlay.IsAutoPlay)
            {
                foreach (Creature enemy in CombatState.HittableEnemies)
                {
                    if (enemy != cardPlay.Target)
                    {
                        await CardCmd.AutoPlay(choiceContext, cardPlay.Card, enemy);
                    }
                }
            }
            
        }
    }
}
