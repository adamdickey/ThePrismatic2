using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Enchantments;

namespace ThePrismatic2.ThePrismatic2Code.Relics;

public sealed class SwordOfInk: ThePrismatic2Relic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    //public override string PackedIconPath => "res://images/atlases/relic_atlas.sprites/burning_blood.tres";
    //protected override string PackedIconOutlinePath => "res://images/atlases/relic_outline_atlas.sprites/burning_blood.tres";
    //protected override string BigIconPath => "res://images/relics/burning_blood.png";

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([ 
        ..HoverTipFactory.FromForge(),
        ..HoverTipFactory.FromEnchantment<Oily>(DynamicVars["Oily"].IntValue)
    ]);
    
    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new ForgeVar(20),
        new DynamicVar("Oily", 3)
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
                    CardCmd.Enchant<Oily>(card, DynamicVars["Oily"].BaseValue);
                }
            }
        }
    }
    
    public override async Task AfterDamageGiven(PlayerChoiceContext choiceContext, Creature? dealer, DamageResult result, ValueProp props, Creature target, CardModel? cardSource)
    {
        if (dealer != null && cardSource is SovereignBlade && (dealer == Owner.Creature || dealer.PetOwner?.Creature == Owner.Creature) && props.IsPoweredAttack() && result.TotalDamage > 0)
        {
            await ForgeCmd.Forge(DynamicVars.Forge.BaseValue, Owner, this);
        }
    }
}