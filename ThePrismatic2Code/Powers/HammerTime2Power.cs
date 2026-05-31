using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class HammerTime2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/hammer_time_power.png-62dff9a13fe845c0d074b55b00afb7d8.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/hammer_time_power.png-62dff9a13fe845c0d074b55b00afb7d8.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromForge();

    public override async Task AfterForge(decimal amount, Player forger, AbstractModel? source)
    {
        if (source is HammerTimePower || forger != Owner.Player)
        {
            return;
        }
        IEnumerable<Player> enumerable = CombatState.Players.Where(p => p.Creature.IsAlive && p != forger);
        foreach (Player item in enumerable)
        {
            await ForgeCmd.Forge(amount, item, this);
        }
    }
    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Type == CardType.Attack)
        {
            Flash();
            if (Owner.Player != null) await ForgeCmd.Forge(Amount, Owner.Player, this);
        }
    }
}
