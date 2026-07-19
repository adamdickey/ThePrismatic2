using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;


namespace ThePrismatic2.ThePrismatic2Code.Relics;

public sealed class RingRemnant: ThePrismatic2Relic
{
    public override RelicRarity Rarity => RelicRarity.Starter;
    public override string PackedIconPath => "res://images/atlases/relic_atlas.sprites/ring_of_the_snake.tres";
    protected override string PackedIconOutlinePath => "res://images/atlases/relic_outline_atlas.sprites/ring_of_the_snake.tres";
    protected override string BigIconPath => "res://images/relics/ring_of_the_snake.png";

    public override decimal ModifyHandDraw(Player player, decimal count)
    {
        if (Owner.PlayerCombatState != null && (player != Owner || Owner.PlayerCombatState.TurnNumber > 1))
        {
            return count;
        }
        return count + 1;
    }
}