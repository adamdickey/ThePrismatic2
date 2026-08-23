using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace ThePrismatic2.ThePrismatic2Code.Relics;

public sealed class SadisticDagger: ThePrismatic2Relic
{
    public override RelicRarity Rarity => RelicRarity.Uncommon;
    //public override string PackedIconPath => "res://images/atlases/relic_atlas.sprites/burning_blood.tres";
    //protected override string PackedIconOutlinePath => "res://images/atlases/relic_outline_atlas.sprites/burning_blood.tres";
    //protected override string BigIconPath => "res://images/relics/burning_blood.png";

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Damage", 3m));

    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (amount != 0m && power.GetTypeForAmount(amount) == PowerType.Debuff && power.Owner.IsEnemy && applier == Owner.Creature && power is not ITemporaryPower)
        {
            await CreatureCmd.Damage(choiceContext, power.Owner, DynamicVars["Damage"].BaseValue, ValueProp.Unpowered, Owner.Creature, null);
        }
    }
}