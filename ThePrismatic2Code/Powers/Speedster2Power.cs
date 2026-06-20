using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class Speedster2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/speedster_power.png-744bf6148ea6d99d5d2befa1d6f2e34a.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/speedster_power.png-744bf6148ea6d99d5d2befa1d6f2e34a.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if (!fromHandDraw && card.Owner.Creature == Owner && card.Owner.Creature.CombatState != null && card.Owner.Creature.CombatState.CurrentSide == card.Owner.Creature.Side)
        {
            VfxCmd.PlayOnCreatureCenters(CombatState.HittableEnemies, "vfx/vfx_attack_slash");
            SfxCmd.Play("slash_attack.mp3");
            await CreatureCmd.Damage(choiceContext, CombatState.HittableEnemies, Amount, ValueProp.Unpowered, Owner, null);
        }
    }
    public override async Task AfterCardGeneratedForCombat(CardModel card, Player? creator)
    {
        Speedster2Power speedster2Power = this;
        if (card.Owner != speedster2Power.Owner.Player || creator != Owner.Player)
            return;
        VfxCmd.PlayOnCreatureCenters(CombatState.HittableEnemies, "vfx/vfx_attack_slash");
        SfxCmd.Play("slash_attack.mp3");
        await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), CombatState.HittableEnemies, Amount, ValueProp.Unpowered, Owner, null);
    }
}
