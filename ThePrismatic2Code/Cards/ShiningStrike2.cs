using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class ShiningStrike2() : ThePrismatic2Card(1, 
    CardType.Attack, CardRarity.Uncommon, 
    TargetType.AnyEnemy)
{
    public override string CustomPortraitPath => "res://.godot/imported/shining_strike.png-88522148d69d858fd3935f058d9d9c5b.ctex";
    public override string PortraitPath => "res://.godot/imported/shining_strike.png-88522148d69d858fd3935f058d9d9c5b.ctex";

    protected override HashSet<CardTag> CanonicalTags => new() { CardTag.Strike };

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new DamageVar(8m, ValueProp.Move),
        new StarsVar(2)
    ]);
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => new _003C_003Ez__ReadOnlySingleElementList<CardKeyword>(Extensions.Keywords.Cunning);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_starry_impact")
            .Execute(choiceContext);
        await PlayerCmd.GainStars(DynamicVars.Stars.BaseValue, Owner);
        if (!Keywords.Contains(CardKeyword.Exhaust) && !ExhaustOnNextPlay)
        {
            await CardPileCmd.Add(this, PileType.Draw, CardPilePosition.Top);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
    }
}