using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using ThePrismatic2.ThePrismatic2Code.Orbs;

namespace ThePrismatic2.ThePrismatic2Code.Relics;

public sealed class CoreRemnant: ThePrismatic2Relic
{

    public override RelicRarity Rarity => RelicRarity.Starter;
    public override string PackedIconPath => "res://images/atlases/relic_atlas.sprites/cracked_core.tres";
    protected override string PackedIconOutlinePath => "res://images/atlases/relic_outline_atlas.sprites/cracked_core.tres";
    protected override string BigIconPath => "res://images/relics/cracked_core.png";

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Lightning", 1m));

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        HoverTipFactory.Static(StaticHoverTip.Channeling),
        HoverTipFactory.FromOrb<SparkOrb>()
    ]);

    public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (participants.Contains(Owner.Creature) && Owner.PlayerCombatState != null && Owner.PlayerCombatState.TurnNumber <= 1)
        {
            for (int i = 0; i < DynamicVars["Lightning"].BaseValue; i++)
            {
                await OrbCmd.Channel<SparkOrb>(new BlockingPlayerChoiceContext(), Owner);
            }
        }
    }
}