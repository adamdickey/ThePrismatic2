using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace ThePrismatic2.ThePrismatic2Code.Relics;

public sealed class BurningRemnant: ThePrismatic2Relic
{
    public override RelicRarity Rarity => RelicRarity.Starter;
    public override string PackedIconPath => "res://images/atlases/relic_atlas.sprites/burning_blood.tres";
    protected override string PackedIconOutlinePath => "res://images/atlases/relic_outline_atlas.sprites/burning_blood.tres";
    protected override string BigIconPath => "res://images/relics/burning_blood.png";

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new HealVar(1m));

    public override async Task AfterPlayerTurnStartLate(PlayerChoiceContext choiceContext, Player player)
    {
        if (player == Owner && Owner.PlayerCombatState != null && Owner.PlayerCombatState.TurnNumber <= 1)
        {
            await CreatureCmd.Heal(Owner.Creature, DynamicVars.Heal.IntValue);
        }
    }
}