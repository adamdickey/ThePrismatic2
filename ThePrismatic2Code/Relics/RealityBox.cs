using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;

namespace ThePrismatic2.ThePrismatic2Code.Relics;

public sealed class RealityBox: ThePrismatic2Relic
{
    public override RelicRarity Rarity => RelicRarity.Rare;
    public override string PackedIconPath => "ThePrismatic2/images/relics/RealityBox.png";
    protected override string PackedIconOutlinePath => "ThePrismatic2/images/relics/RealityBox_Outline.png";
    protected override string BigIconPath => "ThePrismatic2/images/relics/big/RealityBox.png";

    public override Task AfterCardGeneratedForCombat(CardModel card, Player? creator)
    {
        if (creator == null || creator != Owner) return Task.CompletedTask;
        CardCmd.Upgrade(card);
        return Task.CompletedTask;
    }
}