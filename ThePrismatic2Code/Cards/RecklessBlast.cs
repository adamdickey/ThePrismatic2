using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Extensions;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

public class RecklessBlast() : ThePrismatic2Card(0,
    CardType.Attack, CardRarity.Ancient,
    TargetType.AnyEnemy)
{
    public override string CustomPortraitPath => $"PrismaticBlast.png".BigCardImagePath();
    public override string PortraitPath => $"PrismaticBlast.png".CardImagePath();
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(Extensions.Keywords.Bleed));

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new DamageVar(8m, ValueProp.Move),
        new SummonVar(1m),
        new HpLossVar(1m),
        new RepeatVar(3)
    ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target);

        // The whole card - summon, self damage and attack - runs Repeat times.
        for (int i = 0; i < DynamicVars.Repeat.IntValue; i++)
        {
            // Stop early rather than summoning and bleeding for hits on a corpse.
            if (play.Target is not { IsAlive: true }) break;

            await OstyCmd.Summon(choiceContext, Owner, DynamicVars.Summon.BaseValue, this);
            if (!Osty.CheckMissingWithAnim(Owner) && Owner.Osty != null)
            {
                VfxCmd.PlayOnCreatureCenter(Owner.Osty, "vfx/vfx_bloody_impact");
                await CreatureCmd.Damage(choiceContext, Owner.Osty, DynamicVars.HpLoss.BaseValue, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this);
            }
            else
            {
                VfxCmd.PlayOnCreatureCenter(Owner.Creature, "vfx/vfx_bloody_impact");
                await CreatureCmd.Damage(choiceContext, Owner.Creature, DynamicVars.HpLoss.BaseValue, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this);
            }
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target)
                .WithHitFx("vfx/vfx_attack_slash", null, "slash_attack.mp3")
                .Execute(choiceContext);
        }

        // Summoning and then bleeding for the same amount leaves a freshly summoned Osty sitting
        // on exactly 0 HP, and nothing reaps him - he stays standing and still counts as alive.
        // Finish him off explicitly.
        if (Owner.Osty is { CurrentHp: <= 0 } deadOsty)
        {
            await CreatureCmd.Kill(deadOsty, true);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Repeat.UpgradeValueBy(1m);
    }
}