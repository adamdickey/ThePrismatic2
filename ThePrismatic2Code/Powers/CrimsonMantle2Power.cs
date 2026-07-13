using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class CrimsonMantle2Power : ThePrismatic2Power
{
    
    public override string CustomPackedIconPath => "res://.godot/imported/crimson_mantle_power.png-042f8d3cf6d623188943f1fc6a3c9c47.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/crimson_mantle_power.png-042f8d3cf6d623188943f1fc6a3c9c47.s3tc.ctex";

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        HoverTipFactory.Static(StaticHoverTip.Block),
        HoverTipFactory.FromKeyword(Extensions.Keywords.Bleed)
    ]);

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new DamageVar("SelfDamage", 0m, ValueProp.Unblockable | ValueProp.Unpowered));

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player == Owner.Player)
        {
            Flash();
            DamageVar damageVar = (DamageVar)DynamicVars["SelfDamage"];
            if (!Osty.CheckMissingWithAnim(player) && player.Osty != null)
            {
                await CreatureCmd.Damage(choiceContext, player.Osty, damageVar.BaseValue, damageVar.Props, Owner, null);
            }
            else
            {
                await CreatureCmd.Damage(choiceContext, Owner, damageVar.BaseValue, damageVar.Props, Owner, null);
            }
            await CreatureCmd.GainBlock(Owner, Amount, ValueProp.Unpowered, null);
        }
    }

    public void IncrementSelfDamage()
    {
        AssertMutable();
        DynamicVars["SelfDamage"].BaseValue++;
    }
}
