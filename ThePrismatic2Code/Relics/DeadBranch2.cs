using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace ThePrismatic2.ThePrismatic2Code.Relics;

public sealed class DeadBranch2: ThePrismatic2Relic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    public override string PackedIconPath => "ThePrismatic2/images/relics/DeadBranch.png";
    protected override string PackedIconOutlinePath => "ThePrismatic2/images/relics/DeadBranch_Outline.png";
    protected override string BigIconPath => "ThePrismatic2/images/relics/big/DeadBranch.png";

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Exhaust));

    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
    {
        CardModel? randomCard = CardFactory.GetForCombat(Owner, Owner.Character.CardPool.GetUnlockedCards(Owner.UnlockState, Owner.RunState.CardMultiplayerConstraint), 1, Owner.RunState.Rng.CombatCardGeneration).FirstOrDefault();
        if (randomCard != null)
        {
            Flash();
            await CardPileCmd.AddGeneratedCardToCombat(randomCard, PileType.Hand, Owner);
        }
    }
}