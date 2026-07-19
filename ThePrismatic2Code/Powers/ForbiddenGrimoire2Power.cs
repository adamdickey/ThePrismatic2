using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Rooms;
using ThePrismatic2.ThePrismatic2Code.Extensions;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class ForbiddenGrimoire2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/forbidden_grimoire_power.png-9fbaf09ca7bd8ed45c8e048879dffac5.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/forbidden_grimoire_power.png-9fbaf09ca7bd8ed45c8e048879dffac5.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Transform));

    public override Task AfterCombatEnd(CombatRoom room)
    {
        for (int i = 0; i < Amount; i++)
        {
            if (Owner.Player != null) room.AddExtraReward(Owner.Player, new CardTransformReward(Owner.Player));
        }
        return Task.CompletedTask;
    }
}
