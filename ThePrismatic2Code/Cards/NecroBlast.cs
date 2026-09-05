using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Extensions;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

public class NecroBlast() : ThePrismatic2Card(1,
    CardType.Attack, CardRarity.Ancient,
    TargetType.AnyEnemy)
{
    public override string CustomPortraitPath => $"PrismaticBlast.png".BigCardImagePath();
    public override string PortraitPath => $"PrismaticBlast.png".CardImagePath();
    protected override HashSet<CardTag> CanonicalTags => [CardTag.OstyAttack];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.SummonDynamic, DynamicVars.Summon));

    //works like Unleash2, except deals double damage vs summon number on upgrade
    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new SummonVar(10m),
        new CalculationBaseVar(10m),
        new ExtraDamageVar(1m).FromOsty(),
        // Damage is CalculationBase + ExtraDamage * multiplier. The multiplier is what Osty's HP
        // WILL be once this card summons, so the number on the card matches the hit you get.
        new CalculatedDamageVar(ValueProp.Move).FromOsty().WithMultiplier(delegate(CardModel card, Creature? _)
        {
            Creature? osty = card.Owner.Osty;
            decimal currentHp = osty is { IsAlive: true } ? osty.CurrentHp : 0m;
            return currentHp + card.DynamicVars.Summon.BaseValue;
        })
    ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target);
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await OstyCmd.Summon(choiceContext, Owner, DynamicVars.Summon.BaseValue, this);
        if (!Osty.CheckMissingWithAnim(Owner) && Owner.Osty != null)
        {
            // By now the summon has happened, so Osty's HP already includes it and the
            // multiplier's look-ahead has to be taken back out - once per point of ExtraDamage.
            decimal alreadySummoned = DynamicVars.ExtraDamage.BaseValue * DynamicVars.Summon.BaseValue;
            await DamageCmd.Attack(DynamicVars.CalculatedDamage.Calculate(play.Target) - alreadySummoned)
                .FromOsty(Owner.Osty, this)
                .Targeting(play.Target)
                .WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
                .Execute(choiceContext);
        }
    }

    protected override void OnUpgrade()
    {
        // Base stays at 10; Osty's HP is worth double.
        DynamicVars.ExtraDamage.UpgradeValueBy(1m);
    }
}
