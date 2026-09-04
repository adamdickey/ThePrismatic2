using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Orbs;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class Speedster2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/speedster_power.png-744bf6148ea6d99d5d2befa1d6f2e34a.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/speedster_power.png-744bf6148ea6d99d5d2befa1d6f2e34a.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        HoverTipFactory.Static(StaticHoverTip.Channeling),
        HoverTipFactory.FromOrb<LightningOrb>()
    ]);

    private int _cardsDrawn;

    public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if (!fromHandDraw && card.Owner.Creature == Owner && card.Owner.Creature.CombatState != null && card.Owner.Creature.CombatState.CurrentSide == card.Owner.Creature.Side)
        {
            _cardsDrawn++;
            if (_cardsDrawn >= 2)
            {
                _cardsDrawn -= 2;
                for (int i = 0; i < Amount; i++)
                {
                    if (Owner.Player != null) await OrbCmd.Channel<LightningOrb>(choiceContext, Owner.Player);
                }
            }
        }
    }
}
