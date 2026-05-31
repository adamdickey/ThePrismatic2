using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Charge2() : ThePrismatic2Card(1, 
    CardType.Skill, CardRarity.Uncommon, 
    TargetType.Self)
{
    public override string CustomPortraitPath => "res://.godot/imported/charge.png-bb8f6a24b37ecdaae96ba491cb0fa8ff.ctex";
    public override string PortraitPath => "res://.godot/imported/charge.png-bb8f6a24b37ecdaae96ba491cb0fa8ff.ctex";

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<MinionDiveBomb>(IsUpgraded));

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new CardsVar(2));

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        List<CardModel> cardsIn = (from c in PileType.Draw.GetPile(Owner).Cards
            orderby c.Rarity, c.Id
            select c).ToList();
        List<CardModel> list = (await CardSelectCmd.FromSimpleGrid(choiceContext, cardsIn, Owner, new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, DynamicVars.Cards.IntValue))).ToList();
        foreach (CardModel item in list)
        {
            await CardCmd.Exhaust(choiceContext, item);
        }
        foreach (CardModel unused in list)
        {
            CardModel card = CombatState.CreateCard<MinionDiveBomb>(Owner);
            if (IsUpgraded)
            {
                CardCmd.Upgrade(card);
            }
            CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Draw, addedByPlayer: true));
        }
    }
}