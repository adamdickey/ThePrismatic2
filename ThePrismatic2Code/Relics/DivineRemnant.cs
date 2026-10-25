using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;

namespace ThePrismatic2.ThePrismatic2Code.Relics;

public sealed class DivineRemnant: ThePrismatic2Relic
{
    public override RelicRarity Rarity => RelicRarity.Starter;
    public override string PackedIconPath => "ThePrismatic2/images/relics/DivineRemnant.png";
    protected override string PackedIconOutlinePath => "ThePrismatic2/images/relics/DivineRemnant_Outline.png";
    protected override string BigIconPath => "ThePrismatic2/images/relics/big/DivineRemnant.png";

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new StarsVar(1));

    public override async Task AfterRoomEntered(AbstractRoom room)
    {
        if (room is CombatRoom)
        {
            await PlayerCmd.GainStars(DynamicVars.Stars.BaseValue, Owner);
        }
    }
}