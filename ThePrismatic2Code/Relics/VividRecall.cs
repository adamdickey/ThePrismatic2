using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;

namespace ThePrismatic2.ThePrismatic2Code.Relics;

public sealed class VividRecall: ThePrismatic2Relic
{
    public override RelicRarity Rarity => RelicRarity.Starter;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Relics", 3m));

    public override Task AfterObtained()
    {
        ModelId id = ModelDb.Relic<BurningRemnant>().Id;
        RelicModel? relicById = Owner.GetRelicById(id);
        if (relicById != null)
        {
            ModelId id2 = ModelDb.Relic<BurningBlood>().Id;
            RelicModel replace = ModelDb.GetById<RelicModel>(id2).ToMutable();
            RelicCmd.Replace(relicById, replace);
        }
        
        ModelId id3 = ModelDb.Relic<RingRemnant>().Id;
        RelicModel? relicById2 = Owner.GetRelicById(id3);
        if (relicById2 != null)
        {
            ModelId id4 = ModelDb.Relic<RingOfTheSnake>().Id;
            RelicModel replace2 = ModelDb.GetById<RelicModel>(id4).ToMutable();
            RelicCmd.Replace(relicById2, replace2);
        }
        
        ModelId id5 = ModelDb.Relic<DivineRemnant>().Id;
        RelicModel? relicById3 = Owner.GetRelicById(id5);
        if (relicById3 != null)
        {
            ModelId id6 = ModelDb.Relic<DivineRight>().Id;
            RelicModel replace3 = ModelDb.GetById<RelicModel>(id6).ToMutable();
            RelicCmd.Replace(relicById3, replace3);
        }
        
        ModelId id7 = ModelDb.Relic<PhylacteryRemnant>().Id;
        RelicModel? relicById4 = Owner.GetRelicById(id7);
        if (relicById4 != null)
        {
            ModelId id8 = ModelDb.Relic<BoundPhylactery>().Id;
            RelicModel replace4 = ModelDb.GetById<RelicModel>(id8).ToMutable();
            RelicCmd.Replace(relicById4, replace4);
        }
        
        ModelId id9 = ModelDb.Relic<CoreRemnant>().Id;
        RelicModel? relicById5 = Owner.GetRelicById(id9);
        if (relicById5 != null)
        {
            ModelId id10 = ModelDb.Relic<CrackedCore>().Id;
            RelicModel replace5 = ModelDb.GetById<RelicModel>(id10).ToMutable();
            RelicCmd.Replace(relicById5, replace5);
        }
        return Task.CompletedTask;
    }
}