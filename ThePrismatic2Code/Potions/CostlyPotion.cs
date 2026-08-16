using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace ThePrismatic2.ThePrismatic2Code.Potions;

public sealed class CostlyPotion : ThePrismatic2Potion
{
    public override string CustomPackedImagePath => "res://.godot/imported/crystal_sphere_rare_potion.png-c44b696ef7c6c7bc521c95076b865559.ctex";
    
    //public override string CustomPackedOutlinePath => "res://.godot/imported/potion_placeholder.png-a1ad2b9e149feb01c6a9fce0de02ae61.ctex";
    public override PotionRarity Rarity => PotionRarity.Rare;

    public override PotionUsage Usage => PotionUsage.CombatOnly;

    public override TargetType TargetType => TargetType.Self;

    public override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(Extensions.Keywords.Costly));

    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        List<CardModel> cards = CardFactory.GetDistinctForCombat(Owner, from c in Owner.Character.CardPool.GetUnlockedCards(Owner.UnlockState, Owner.RunState.CardMultiplayerConstraint)
            where Math.Max(0, c.EnergyCost.Canonical) + Math.Max(0, c.CanonicalStarCost) >= 2
            select c, 3, Owner.RunState.Rng.CombatCardGeneration).ToList();
        CardModel? cardModel = await CardSelectCmd.FromChooseACardScreen(choiceContext, cards, Owner, canSkip: true);
        if (cardModel != null)
        {
            cardModel.SetToFreeThisTurn();
            await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Hand, Owner);
        }
    }
}
