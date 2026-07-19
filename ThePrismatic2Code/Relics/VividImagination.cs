using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace ThePrismatic2.ThePrismatic2Code.Relics;

public sealed class VividImagination: ThePrismatic2Relic
{
    public override RelicRarity Rarity => RelicRarity.Starter;
    
    public override RelicModel GetUpgradeReplacement() => ModelDb.Relic<VividRecall>();
    
    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Relics", 3m));
    
    private int[] _relicInts = [];
    
    [SavedProperty]
    private int[] RelicInts
    {
        get => _relicInts;
        set
        {
            AssertMutable();
            _relicInts = value;
        }
    }

    public override Task AfterObtained()
    {
        int[] allInts = [0, 1, 2, 3, 4];
        int[] selectedInts = allInts.OrderBy(_ => Random.Shared.Next()).Take(DynamicVars["Relics"].IntValue).ToArray();
        foreach (int index in selectedInts)
        {
            RelicInts = RelicInts.Append(index).ToArray();
        }
        return Task.CompletedTask;
    }
    
    public override async Task AfterRoomEntered(AbstractRoom room)
    {
        if (Owner.RunState is { CurrentActIndex: 0, ActFloor: 1 })
        {
            foreach (RelicModel relic in Owner.Relics)
            {
                if (relic is BurningRemnant or RingRemnant or DivineRemnant or PhylacteryRemnant or CoreRemnant)
                {
                    return;
                }
            }
            List<RelicModel> list = [ModelDb.Relic<BurningRemnant>(), ModelDb.Relic<RingRemnant>(), ModelDb.Relic<DivineRemnant>(), ModelDb.Relic<PhylacteryRemnant>(), ModelDb.Relic<CoreRemnant>()];
            List<RelicModel> list2 = RelicInts.Select(i => list[i]).ToList();
            foreach (RelicModel relic in list2)
            {
                if (relic == ModelDb.Relic<BurningRemnant>())
                {
                    await RelicCmd.Obtain<BurningRemnant>(Owner);
                    continue;
                }
                if (relic == ModelDb.Relic<RingRemnant>())
                {
                    await RelicCmd.Obtain<RingRemnant>(Owner);
                    continue;
                }
                if (relic == ModelDb.Relic<DivineRemnant>())
                {
                    await RelicCmd.Obtain<DivineRemnant>(Owner);
                    continue;
                }
                if (relic == ModelDb.Relic<PhylacteryRemnant>())
                {
                    await RelicCmd.Obtain<PhylacteryRemnant>(Owner);
                    continue;
                }
                if (relic == ModelDb.Relic<CoreRemnant>())
                {
                    await RelicCmd.Obtain<CoreRemnant>(Owner);
                }
            }
        }
    }
}