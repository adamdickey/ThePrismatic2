using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Melancholy2() : ThePrismatic2Card(3, 
    CardType.Skill, CardRarity.Uncommon, 
    TargetType.Self)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<NecrobinderCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/melancholy.png-3b295ba238dd68f34ee67a7eab484483.ctex";
    public override string PortraitPath => "res://.godot/imported/melancholy.png-3b295ba238dd68f34ee67a7eab484483.ctex";
    
    public override bool GainsBlock => true;

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new BlockVar(13m, ValueProp.Move),
        new EnergyVar(1)
    ]);

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(EnergyHoverTip);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
    }
    
    public override Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        CardPile? pile = Pile;
        if (pile is not { IsCombatPile: true })
        {
            return Task.CompletedTask;
        }
        if (target == Owner.Creature && result.UnblockedDamage > 0)
        {
            EnergyCost.AddThisCombat(-DynamicVars.Energy.IntValue);
        }
        return Task.CompletedTask;
    }
    
    public override Task AfterCurrentHpChanged(Creature creature, decimal delta)
    {
        CardPile? pile = Pile;
        if (pile is not { IsCombatPile: true })
        {
            return Task.CompletedTask;
        }
        if (delta < 0 && creature.Monster is Osty && creature.PetOwner == Owner)
        {
            EnergyCost.AddThisCombat(-DynamicVars.Energy.IntValue);
        }
        return Task.CompletedTask;
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(4m);
    }
}