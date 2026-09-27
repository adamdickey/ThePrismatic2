using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Extensions;
using CardKeyword = MegaCrit.Sts2.Core.Entities.Cards.CardKeyword;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

public class CoordinatedStrike() : ThePrismatic2Card(1,
    CardType.Attack, CardRarity.None,
    TargetType.AnyEnemy), ITranscendenceCard
{
    public override string CustomPortraitPath => $"PrismaticStrike.png".BigCardImagePath();
    public override string PortraitPath => $"PrismaticStrike.png".CardImagePath();
    
    public override bool IsBasicStrikeOrDefend => false;
    
    public override bool CanBeGeneratedInCombat => false;
    
    public CardModel GetTranscendenceTransformedCard() => ModelDb.Card<CoordinatedBlast>();
    protected override bool ShouldGlowGoldInternal => !Osty.CheckMissingWithAnim(Owner);
    protected override HashSet<CardTag> CanonicalTags => [CardTag.OstyAttack, CardTag.Strike];
    public override IEnumerable<CardKeyword> CanonicalKeywords => new _003C_003Ez__ReadOnlySingleElementList<CardKeyword>(Extensions.Keywords.DualWield);

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new DamageVar(8m, ValueProp.Move),
        new OstyDamageVar(4m, ValueProp.Move)
    ]);

    private bool _ostyPlay;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target);
        if (_ostyPlay && Owner.Osty != null)
        {
            await DamageCmd.Attack(DynamicVars.OstyDamage.BaseValue).FromOsty(Owner.Osty, this)
                .Targeting(play.Target)
                .WithHitFx("vfx/vfx_attack_blunt")
                .Execute(choiceContext);
        }
        else
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
        DynamicVars.OstyDamage.UpgradeValueBy(1m);
    }
    
    public override async Task AfterCardPlayedLate(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card == this && !Osty.CheckMissingWithAnim(Owner) && !_ostyPlay)
        {
            _ostyPlay = true;
            await CardCmd.AutoPlay(choiceContext, this, cardPlay.Target);
        }
        else
        {
            _ostyPlay = false;
        }
    }
}