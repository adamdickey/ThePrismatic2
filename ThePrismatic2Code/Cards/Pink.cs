using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Pink() : ThePrismatic2Card(-1, 
    CardType.Status, CardRarity.Status, 
    TargetType.None)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<NecrobinderCardPool>();
    
    public override int MaxUpgradeLevel => 0;

    public override bool CanBeGeneratedInCombat => false;
}