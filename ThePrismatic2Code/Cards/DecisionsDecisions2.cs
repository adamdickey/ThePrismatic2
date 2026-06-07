using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class DecisionsDecisions2() : ThePrismatic2Card(0, 
    CardType.Skill, CardRarity.Rare, 
    TargetType.Self)
{
    public override string CustomPortraitPath => "res://.godot/imported/decisions_decisions.png-dd9e5bc21a8a2958e7af67ea73867136.ctex";
    public override string PortraitPath => "res://.godot/imported/decisions_decisions.png-dd9e5bc21a8a2958e7af67ea73867136.ctex";
    
    public override int CanonicalStarCost => 6;

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new CardsVar(3),
        new RepeatVar(3)
    ]);

    public override IEnumerable<CardKeyword> CanonicalKeywords => new _003C_003Ez__ReadOnlyArray<CardKeyword>([
        CardKeyword.Exhaust,
        Extensions.Keywords.Starbound
        ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);
        CardSelectorPrefs prefs = new CardSelectorPrefs(SelectionScreenPrompt, 1)
        {
            PretendCardsCanBePlayed = true
        };
        CardModel? card = (await CardSelectCmd.FromHand(choiceContext, Owner, prefs, c => c.Type == CardType.Skill && !c.Keywords.Contains(CardKeyword.Unplayable), this)).FirstOrDefault();
        if (card != null)
        {
            for (int i = 0; i < DynamicVars.Repeat.IntValue; i++)
            {
                await CardCmd.AutoPlay(choiceContext, card, null);
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(2m);
    }
}