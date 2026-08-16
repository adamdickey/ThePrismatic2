using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using ThePrismatic2.ThePrismatic2Code.Powers;

namespace ThePrismatic2.ThePrismatic2Code.Potions;

public sealed class ExposedPotion : ThePrismatic2Potion
{
    public override string CustomPackedImagePath => "res://.godot/imported/crystal_sphere_common_potion.png-f744c5cace1a6d462cb56c6952e27270.ctex";
    
    //public override string CustomPackedOutlinePath => "res://.godot/imported/potion_placeholder.png-a1ad2b9e149feb01c6a9fce0de02ae61.ctex";
    public override PotionRarity Rarity => PotionRarity.Common;

    public override PotionUsage Usage => PotionUsage.CombatOnly;

    public override TargetType TargetType => TargetType.AnyEnemy;

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Exposed", 5m));

    public override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<ExposedPower>());

    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        AssertValidForTargetedPotion(target);
        NCombatRoom.Instance?.PlaySplashVfx(target, new Color("fd21fd"));
        await PowerCmd.Apply<ExposedPower>(choiceContext, target, DynamicVars["Exposed"].BaseValue, Owner.Creature, null);
    }
}
