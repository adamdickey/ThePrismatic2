using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class SentryMode2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/sentry_mode_power.png-be5c916a930262d120eb1c384c513d41.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/sentry_mode_power.png-be5c916a930262d120eb1c384c513d41.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<SweepingGaze>());

    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, CombatState combatState)
    {
        if (player == Owner.Player)
        {
            for (int i = 0; i < Amount; i++)
            {
                await OstyCmd.Summon(choiceContext, Owner.Player, 1, this);
                CardModel card = combatState.CreateCard<SweepingGaze>(Owner.Player);
                await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, addedByPlayer: true);
            }
        }
    }
}
