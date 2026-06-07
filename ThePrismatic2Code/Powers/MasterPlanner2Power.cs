using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class MasterPlanner2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/master_planner_power.png-cf7df04d84d4c262be9a1e00e3311bd9.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/master_planner_power.png-cf7df04d84d4c262be9a1e00e3311bd9.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Single;

    public override Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner.Player)
        {
            return Task.CompletedTask;
        }
        if (cardPlay.Card.Type != CardType.Skill)
        {
            return Task.CompletedTask;
        }
        Flash();
        CardCmd.ApplyKeyword(cardPlay.Card, Extensions.Keywords.Cunning);
        CardCmd.ApplyKeyword(cardPlay.Card, Extensions.Keywords.Starbound);
        return Task.CompletedTask;
    }
}
