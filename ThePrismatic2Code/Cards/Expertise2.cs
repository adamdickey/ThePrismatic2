using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Expertise2() : ThePrismatic2Card(1, 
    CardType.Skill, CardRarity.Uncommon, 
    TargetType.Self)
{
    public override string CustomPortraitPath => "res://.godot/imported/expertise.png-06f1cd9ff165af60413927e39a04ef5c.ctex";
    public override string PortraitPath => "res://.godot/imported/expertise.png-06f1cd9ff165af60413927e39a04ef5c.ctex";

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
            new CardsVar(6),
            new StarsVar(1),
            new CalculationBaseVar(0m),
            new CalculationExtraVar(1m),
            new CalculatedVar("CalculatedStars").WithMultiplier(delegate(CardModel card, Creature? _)
            {
                if (card.Owner.PlayerCombatState == null) return 0;
                return Math.Max(0m, Math.Floor((decimal)(card.Owner.PlayerCombatState.Hand.Cards.Count-1)/2));
            })
        ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        decimal baseValue = DynamicVars.Cards.BaseValue;
        if (Owner.PlayerCombatState != null)
        {
            int count = Owner.PlayerCombatState.Hand.Cards.Count;
            // ReSharper disable once PossibleLossOfFraction
            decimal starCount = Math.Max(0m, count/2);
            await PlayerCmd.GainStars(starCount, Owner);
        
            decimal count2 = Math.Max(0m, baseValue - count);
            await CardPileCmd.Draw(choiceContext, count2, Owner);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1m);
    }
}