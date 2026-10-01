using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using ThePrismatic2.ThePrismatic2Code.Enchantments;

namespace ThePrismatic2.ThePrismatic2Code.Relics;

public sealed class BloodySabertooth: ThePrismatic2Relic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        ..HoverTipFactory.FromForge(),
        ..HoverTipFactory.FromEnchantment<TanxsSoul>(DynamicVars["Soul"].IntValue)
    ]);

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new ForgeVar(20),
        new DynamicVar("Soul", 1)
    ]);

    public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (participants.Contains(Owner.Creature) && Owner.PlayerCombatState is { TurnNumber: <= 1 })
        {
            Flash();
            await ForgeCmd.Forge(DynamicVars.Forge.BaseValue, Owner, this);
            foreach (CardModel card in Owner.PlayerCombatState.AllCards)
            {
                if (card is SovereignBlade)
                {
                    CardCmd.Enchant<TanxsSoul>(card, DynamicVars["Soul"].BaseValue);
                }
            }
        }
    }
}
