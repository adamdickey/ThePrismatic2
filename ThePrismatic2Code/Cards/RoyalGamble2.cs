using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class RoyalGamble2() : ThePrismatic2Card(0, 
    CardType.Skill, CardRarity.Uncommon, 
    TargetType.Self)
{
    public override string CustomPortraitPath => "res://.godot/imported/royal_gamble.png-da80e36357cce992acb57ba43bbe77b6.ctex";
    public override string PortraitPath => "res://.godot/imported/royal_gamble.png-da80e36357cce992acb57ba43bbe77b6.ctex";

    public override int CanonicalStarCost => 3;

    public override IEnumerable<CardKeyword> CanonicalKeywords => new _003C_003Ez__ReadOnlyArray<CardKeyword>([
        CardKeyword.Exhaust,
        Extensions.Keywords.Starbound
        ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.AttackAnimDelay);
        IEnumerable<CardModel> cards = PileType.Hand.GetPile(Owner).Cards;
        int cardsToDraw = cards.Count();
        await CardCmd.DiscardAndDraw(choiceContext, cards, cardsToDraw);
        await PlayerCmd.GainStars(cardsToDraw, Owner);
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }
}