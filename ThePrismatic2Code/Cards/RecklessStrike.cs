using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Extensions;

namespace ThePrismatic2.ThePrismatic2Code.Cards;


public class RecklessStrike() : ThePrismatic2Card(0,
    CardType.Attack, CardRarity.Basic,
    TargetType.AnyEnemy)
{
    public override string CustomPortraitPath => $"PrismaticStrike.png".BigCardImagePath();
    public override string PortraitPath => $"PrismaticStrike.png".CardImagePath();
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Strike];

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new DamageVar(6m, ValueProp.Move));


    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target);
        if (!Osty.CheckMissingWithAnim(Owner) && Owner.Osty != null)
        {
            VfxCmd.PlayOnCreatureCenter(Owner.Osty, "vfx/vfx_bloody_impact");
            await CreatureCmd.Damage(choiceContext, Owner.Osty, 1, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this);
        }
        else
        {
            VfxCmd.PlayOnCreatureCenter(Owner.Creature, "vfx/vfx_bloody_impact");
            await CreatureCmd.Damage(choiceContext, Owner.Creature, 1, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this);
        }
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target)
            .WithHitFx("vfx/vfx_attack_slash", null, "slash_attack.mp3")
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
    }
}