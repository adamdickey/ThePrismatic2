using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace ThePrismatic2.ThePrismatic2Code.Relics;

public sealed class Brimstone2: ThePrismatic2Relic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    public override string PackedIconPath => "res://images/atlases/relic_atlas.sprites/brimstone.tres";
    protected override string PackedIconOutlinePath => "res://images/atlases/relic_outline_atlas.sprites/brimstone.tres";
    protected override string BigIconPath => "res://images/relics/brimstone.png";

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new PowerVar<StrengthPower>("SelfStrength", 2m),
        new PowerVar<FocusPower>(1m),
        new PowerVar<StrengthPower>("EnemyStrength", 1m)
    ]);

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        HoverTipFactory.FromPower<StrengthPower>(),
        HoverTipFactory.FromPower<FocusPower>()
    ]);

    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (participants.Contains(Owner.Creature))
        {
            Flash();
            await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), Owner.Creature, DynamicVars["SelfStrength"].BaseValue, Owner.Creature, null);
            await PowerCmd.Apply<FocusPower>(new ThrowingPlayerChoiceContext(), Owner.Creature, DynamicVars["FocusPower"].BaseValue, Owner.Creature, null);
            IEnumerable<Creature> targets = from c in combatState.GetOpponentsOf(Owner.Creature)
                where c.IsAlive
                select c;
            await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), targets, DynamicVars["EnemyStrength"].BaseValue, null, null);
        }
    }
}