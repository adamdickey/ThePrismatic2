using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Extensions;
using CardKeyword = MegaCrit.Sts2.Core.Entities.Cards.CardKeyword;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

public class StarboundStrike() : ThePrismatic2Card(0,
    CardType.Attack, CardRarity.None,
    TargetType.AnyEnemy)
{
    public override string CustomPortraitPath => $"PrismaticStrike.png".BigCardImagePath();
    public override string PortraitPath => $"PrismaticStrike.png".CardImagePath();

    public override int CanonicalStarCost => 1;
    
    public override bool IsBasicStrikeOrDefend => false;

    protected override HashSet<CardTag> CanonicalTags => [CardTag.Strike];

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new DamageVar(7m, ValueProp.Move));
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => new _003C_003Ez__ReadOnlySingleElementList<CardKeyword>(Extensions.Keywords.Starbound);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
    }
}