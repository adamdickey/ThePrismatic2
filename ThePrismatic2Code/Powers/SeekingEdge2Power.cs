using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class SeekingEdge2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/seeking_edge_power.png-cd5d0a5cb81adc983a1f75365a838706.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/seeking_edge_power.png-cd5d0a5cb81adc983a1f75365a838706.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Single;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(Extensions.Keywords.Costly));

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CardModel? lastPlayedCard = CombatManager.Instance.History.Entries.OfType<CardPlayFinishedEntry>().ElementAtOrDefault(^2)?.CardPlay.Card;
        if (cardPlay.Card is { Type: CardType.Attack, TargetType: TargetType.AnyEnemy } && cardPlay.Card.EnergyCost.GetResolved() + Math.Max(0, cardPlay.Card.LastStarsSpent) >= 2)
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
