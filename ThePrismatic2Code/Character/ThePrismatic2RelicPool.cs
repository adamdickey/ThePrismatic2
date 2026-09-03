using BaseLib.Abstracts;
using ThePrismatic2.ThePrismatic2Code.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using ThePrismatic2.ThePrismatic2Code.Relics;

namespace ThePrismatic2.ThePrismatic2Code.Character;

public class ThePrismatic2RelicPool : CustomRelicPoolModel
{
    public override Color LabOutlineColor => ThePrismatic2.Color;

    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();

    protected override RelicModel[] GenerateAllRelics()
    {
        RelicModel[] relicPool =
        [
            ModelDb.Relic<SadisticDagger>(),
            ModelDb.Relic<RealityBox>(),
            ModelDb.Relic<BagOfDice>(),
            ModelDb.Relic<CostlyForge>(),
            ModelDb.Relic<InnateRelic>(),
            ModelDb.Relic<RedSkull>(),
            ModelDb.Relic<CharonsAshes>(),
            ModelDb.Relic<TwistedFunnel>(),
            ModelDb.Relic<FencingManual>(),
            ModelDb.Relic<Regalite>(),
            ModelDb.Relic<LunarPastry>(),
            ModelDb.Relic<MiniRegent>(),
            ModelDb.Relic<FuneraryMask>(),
            ModelDb.Relic<Bookmark>(),
            ModelDb.Relic<IvoryTile>(),
            ModelDb.Relic<DataDisk>(),
            ModelDb.Relic<EmotionChip>(),
            ModelDb.Relic<PowerCell>(),
            ModelDb.Relic<RunicCapacitor>()
        ];
        return relicPool;
    }   //Note: Add boss relics via AncientRelicInjection.cs, not here. This pool is for non-boss relics only.
}