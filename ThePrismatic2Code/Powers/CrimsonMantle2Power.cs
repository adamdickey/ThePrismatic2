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
    
    private const string _selfDamageKey = "SelfDamage";

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new global::_003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block));

    protected override IEnumerable<DynamicVar> CanonicalVars => new global::_003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new DamageVar("SelfDamage", 0m, ValueProp.Unblockable | ValueProp.Unpowered));

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player == base.Owner.Player)
        {
            Flash();
            DamageVar damageVar = (DamageVar)base.DynamicVars["SelfDamage"];
            if (!Osty.CheckMissingWithAnim(player))
            {
                await CreatureCmd.Damage(choiceContext, player.Osty, damageVar.BaseValue, damageVar.Props, base.Owner, null);
            }
            else
            {
                await CreatureCmd.Damage(choiceContext, base.Owner, damageVar.BaseValue, damageVar.Props, base.Owner, null);
            }
            await CreatureCmd.GainBlock(base.Owner, base.Amount, ValueProp.Unpowered, null);
        }
    }

    public void IncrementSelfDamage()
    {
        AssertMutable();
        base.DynamicVars["SelfDamage"].BaseValue++;
    }
}
