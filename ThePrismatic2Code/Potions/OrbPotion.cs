using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace ThePrismatic2.ThePrismatic2Code.Potions;

public sealed class OrbPotion : ThePrismatic2Potion
{
    public override string CustomPackedImagePath => "res://.godot/imported/discovery_potion.png-63ab67b167186ee1396da433aaeaac19.bptc.ctex";
    
    //public override string CustomPackedOutlinePath => "res://.godot/imported/potion_placeholder.png-a1ad2b9e149feb01c6a9fce0de02ae61.ctex";
    public override PotionRarity Rarity => PotionRarity.Uncommon;

    public override PotionUsage Usage => PotionUsage.CombatOnly;

    public override TargetType TargetType => TargetType.Self;

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new RepeatVar(3));

    public override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Channeling));

    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        for (int i = 0; i < DynamicVars.Repeat.IntValue; i++)
        {
            await OrbCmd.Channel(choiceContext, OrbModel.GetRandomOrb(Owner.RunState.Rng.CombatOrbGeneration).ToMutable(), Owner);
        }
    }
}
