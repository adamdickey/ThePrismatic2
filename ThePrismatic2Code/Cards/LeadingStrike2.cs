using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class LeadingStrike2() : ThePrismatic2Card(1, 
    CardType.Attack, CardRarity.Common, 
    TargetType.AnyEnemy)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<SilentCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/leading_strike.png-bdf93f43bdd3ccd2de60ee616161458e.ctex";
    public override string PortraitPath => "res://.godot/imported/leading_strike.png-bdf93f43bdd3ccd2de60ee616161458e.ctex";

    protected override bool ShouldGlowGoldInternal => !Osty.CheckMissingWithAnim(Owner);
    protected override HashSet<CardTag> CanonicalTags => [ CardTag.Strike, CardTag.OstyAttack ];

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new CardsVar("Shivs", 1),
        new DamageVar(3m, ValueProp.Move),
        new OstyDamageVar(1m, ValueProp.Move)
    ]);
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => new _003C_003Ez__ReadOnlySingleElementList<CardKeyword>(Extensions.Keywords.DualWield);

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<Shiv>());

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
        for (int i = 0; i < DynamicVars["Shivs"].IntValue; i++)
        {
            if (CombatState != null) await Shiv.CreateInHand(Owner, CombatState);
            await Cmd.Wait(0.25f);
        }
        if (!Osty.CheckMissingWithAnim(Owner) && Owner.Osty != null)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target);
            await DamageCmd.Attack(DynamicVars.OstyDamage.BaseValue).FromOsty(Owner.Osty, this)
                .Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_blunt")
                .Execute(choiceContext);
            for (int i = 0; i < DynamicVars["Shivs"].IntValue; i++)
            {
                if (CombatState != null) await Shiv.CreateInHand(Owner, CombatState);
                await Cmd.Wait(0.25f);
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
        DynamicVars.OstyDamage.UpgradeValueBy(1m);
    }
}