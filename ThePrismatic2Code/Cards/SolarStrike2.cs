using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class SolarStrike2() : ThePrismatic2Card(1, 
    CardType.Attack, CardRarity.Common, 
    TargetType.AnyEnemy)
{
    public override string CustomPortraitPath => "res://.godot/imported/solar_strike.png-051b76007e2189629b60e0b412916e44.ctex";
    public override string PortraitPath => "res://.godot/imported/solar_strike.png-051b76007e2189629b60e0b412916e44.ctex";

    protected override HashSet<CardTag> CanonicalTags => new() { CardTag.Strike };

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new DamageVar(8m, ValueProp.Move),
        new StarsVar(1)
    ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
        IEnumerable<CardModel> cards = await CardSelectCmd.FromHandForDiscard(choiceContext, Owner, new CardSelectorPrefs(CardSelectorPrefs.DiscardSelectionPrompt, 1), null, this);
        var cardModels = cards.ToList();
        await CardCmd.Discard(choiceContext, cardModels);
        foreach (CardModel card in cardModels)
        {
            await PlayerCmd.GainStars(card.EnergyCost.Canonical, Owner);
        }


    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
    }
}