using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;

namespace ThePrismatic2.ThePrismatic2Code.Relics;

public sealed class RealityBox: ThePrismatic2Relic
{
    public override RelicRarity Rarity => RelicRarity.Rare;
    //public override string PackedIconPath => "res://images/atlases/relic_atlas.sprites/burning_blood.tres";
    //protected override string PackedIconOutlinePath => "res://images/atlases/relic_outline_atlas.sprites/burning_blood.tres";
    //protected override string BigIconPath => "res://images/relics/burning_blood.png";

    public override Task AfterCardGeneratedForCombat(CardModel card, Player? creator)
    {
        if (creator == null || creator != Owner) return Task.CompletedTask;
        CardCmd.Upgrade(card);
        return Task.CompletedTask;
    }
}