using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;


namespace ThePrismatic2.ThePrismatic2Code.Relics;

public sealed class RingRemnant: ThePrismatic2Relic
{
    public override RelicRarity Rarity => RelicRarity.Starter;
    public override string PackedIconPath => "ThePrismatic2/images/relics/RingRemnant.png";
    protected override string PackedIconOutlinePath => "ThePrismatic2/images/relics/RingRemnant_Outline.png";
    protected override string BigIconPath => "ThePrismatic2/images/relics/big/RingRemnant.png";

    public override decimal ModifyHandDraw(Player player, decimal count)
    {
        if (Owner.PlayerCombatState != null && (player != Owner || Owner.PlayerCombatState.TurnNumber > 1))
        {
            return count;
        }
        return count + 1;
    }
}