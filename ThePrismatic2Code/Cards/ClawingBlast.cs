using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Extensions;
using ThePrismatic2.ThePrismatic2Code.Powers;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

public class ClawingBlast() : ThePrismatic2Card(0,
    CardType.Attack, CardRarity.Ancient,
    TargetType.AnyEnemy)
{
    public override string CustomPortraitPath => $"PrismaticBlast.png".BigCardImagePath();
    public override string PortraitPath => $"PrismaticBlast.png".CardImagePath();

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new DamageVar(12m, ValueProp.Move),
        new PowerVar<Accuracy2Power>(4m)
        ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target)
            .WithHitFx("vfx/vfx_attack_slash", null, "slash_attack.mp3")
            .Execute(choiceContext);
        await PowerCmd.Apply<Accuracy2Power>(choiceContext, Owner.Creature, DynamicVars["Accuracy2Power"].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4m);
        DynamicVars["Accuracy2Power"].UpgradeValueBy(1m);
    }
}