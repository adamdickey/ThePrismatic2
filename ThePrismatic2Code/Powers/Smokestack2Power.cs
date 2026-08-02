using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class Smokestack2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/smokestack_power.png-4a759e5609862177427fac54d8f0c5f8.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/smokestack_power.png-4a759e5609862177427fac54d8f0c5f8.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<PoisonPower>());

    public override async Task AfterCardGeneratedForCombat(CardModel card, Player? creator)
    {
        if (creator == Owner.Player && card.Owner.Creature == Owner)
        {
            Flash();
            await PowerCmd.Apply<PoisonPower>(new ThrowingPlayerChoiceContext(), CombatState.HittableEnemies, Amount, Owner, null);
        }
    }
}
