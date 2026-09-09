using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace ThePrismatic2.ThePrismatic2Code.Relics;

public sealed class PaelsSaliva: ThePrismatic2Relic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    //public override string PackedIconPath => "res://images/atlases/relic_atlas.sprites/burning_blood.tres";
    //protected override string PackedIconOutlinePath => "res://images/atlases/relic_outline_atlas.sprites/burning_blood.tres";
    //protected override string BigIconPath => "res://images/relics/burning_blood.png";

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Channeling));
    
    public override bool ShowCounter => true;
    
    public override int DisplayAmount => Orbs;

    private int _potionsUsed;

    private int _orbs;
    
    [SavedProperty]
    private int Orbs
    {
        get => _orbs;
        set
        {
            AssertMutable();
            _orbs = value;
        }
    }
    
    [SavedProperty]
    private int PotionsUsed
    {
        get => _potionsUsed;
        set
        {
            AssertMutable();
            _potionsUsed = value;
        }
    }
    
    public override Task AfterPotionUsed(PotionModel potion, Creature? target)
    {
        if (potion.Owner == Owner)
        {
            PotionsUsed++;
            if (PotionsUsed >= 2)
            {
                if (!CombatManager.Instance.IsOverOrEnding) Flash();
                Orbs++;
                InvokeDisplayAmountChanged();
                PotionsUsed = 0;
            }
        }
        return Task.CompletedTask;
    }
    
    public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (participants.Contains(Owner.Creature) && Owner.PlayerCombatState is { TurnNumber: <= 1 } && Orbs > 0)
        {
            Flash();
            await OrbCmd.AddSlots(Owner, Orbs);
            for (int i = 0; i < Orbs; i++)
            {
                await OrbCmd.Channel(choiceContext, OrbModel.GetRandomOrb(Owner.RunState.Rng.CombatOrbGeneration).ToMutable(), Owner);
            }
        }
    }
}