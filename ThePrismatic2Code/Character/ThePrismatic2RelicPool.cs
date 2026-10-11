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
            ModelDb.Relic<AlarmClock>(),
            ModelDb.Relic<BejeweledNecklace>(),
            ModelDb.Relic<Binoculars>(),
            ModelDb.Relic<BloodySabertooth>(),
            ModelDb.Relic<Brimstone2>(),
            ModelDb.Relic<BurningRemnant>(),
            ModelDb.Relic<CoreRemnant>(),
            ModelDb.Relic<DeadBranch2>(),
            ModelDb.Relic<DivineRemnant>(),
            ModelDb.Relic<EternalBones>(),
            ModelDb.Relic<FieryCheer>(),
            ModelDb.Relic<GemFragments>(),
            ModelDb.Relic<Kaleidoscope2>(),
            ModelDb.Relic<NoblesAnvil>(),
            ModelDb.Relic<PaelsHeart>(),
            ModelDb.Relic<PhylacteryRemnant>(),
            ModelDb.Relic<PrismaticGem2>(),
            ModelDb.Relic<RealityBox>(),
            ModelDb.Relic<RingRemnant>(),
            ModelDb.Relic<SadisticDagger>(),
            ModelDb.Relic<SnakeEyes>(),
            ModelDb.Relic<StarChild>(),
            ModelDb.Relic<RedSkull>(),
            ModelDb.Relic<CharonsAshes>(),
            ModelDb.Relic<TwistedFunnel>(),
            ModelDb.Relic<FencingManual>(),
            ModelDb.Relic<Regalite>(),
            ModelDb.Relic<LunarPastry>(),
            ModelDb.Relic<FuneraryMask>(),
            ModelDb.Relic<Bookmark>(),
            ModelDb.Relic<IvoryTile>(),
            ModelDb.Relic<DataDisk>(),
            ModelDb.Relic<EmotionChip>(),
            ModelDb.Relic<PowerCell>(),
            ModelDb.Relic<RunicCapacitor>()
        ];
        return relicPool;
    }
}