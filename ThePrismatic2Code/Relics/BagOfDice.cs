using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using ThePrismatic2.ThePrismatic2Code.Powers;

namespace ThePrismatic2.ThePrismatic2Code.Relics;

public sealed class BagOfDice: ThePrismatic2Relic
{
    public override RelicRarity Rarity => RelicRarity.Common;
    //public override string PackedIconPath => "res://images/atlases/relic_atlas.sprites/burning_blood.tres";
    //protected override string PackedIconOutlinePath => "res://images/atlases/relic_outline_atlas.sprites/burning_blood.tres";
    //protected override string BigIconPath => "res://images/relics/burning_blood.png";
    
    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new PowerVar<ExposedPower>(2m));

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<ExposedPower>());

    public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (participants.Contains(Owner.Creature) && Owner.PlayerCombatState is { TurnNumber: <= 1 })
        {
            Flash();
            await PowerCmd.Apply<ExposedPower>(choiceContext, combatState.HittableEnemies, DynamicVars["ExposedPower"].BaseValue, Owner.Creature, null);
        }
    }
}