using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Extensions;
using ThePrismatic2.ThePrismatic2Code.Powers;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

public class ExposingBlast() : ThePrismatic2Card(1,
    CardType.Attack, CardRarity.Ancient,
    TargetType.AnyEnemy)
{
    public override string CustomPortraitPath => $"PrismaticBlast.png".BigCardImagePath();
    public override string PortraitPath => $"PrismaticBlast.png".CardImagePath();
    
    public override bool IsBasicStrikeOrDefend => false;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<ExposedPower>());

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new DamageVar(21m, ValueProp.Move),
        new DynamicVar("Exposed", 99m)
    ]);


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target)
            .WithHitFx("vfx/vfx_attack_slash", null, "slash_attack.mp3")
            .Execute(choiceContext);
        await PowerCmd.Apply<ExposedPower>(choiceContext, play.Target, DynamicVars["Exposed"].BaseValue, Owner.Creature, this);
        
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(9m);
        DynamicVars["Exposed"].UpgradeValueBy(900m);
    }
}