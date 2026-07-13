using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;

namespace ThePrismatic2.ThePrismatic2Code.Relics;

public sealed class DivineRemnant: ThePrismatic2Relic
{
    public override RelicRarity Rarity => RelicRarity.Starter;
    public override string PackedIconPath => "res://images/atlases/relic_atlas.sprites/divine_right.tres";
    protected override string PackedIconOutlinePath => "res://images/atlases/relic_outline_atlas.sprites/divine_right.tres";
    protected override string BigIconPath => "res://images/relics/divine_right.png";

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new StarsVar(1));

    public override async Task AfterRoomEntered(AbstractRoom room)
    {
        if (room is CombatRoom)
        {
            await PlayerCmd.GainStars(DynamicVars.Stars.BaseValue, Owner);
        }
    }
}