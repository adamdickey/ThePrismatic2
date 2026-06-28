using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Refract2() : ThePrismatic2Card(3, 
    CardType.Attack, CardRarity.Uncommon, 
    TargetType.AnyEnemy)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<DefectCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/refract.png-1a2eefec5e3d3b524eb69d13f3fafb2a.ctex";
    public override string PortraitPath => "res://.godot/imported/refract.png-1a2eefec5e3d3b524eb69d13f3fafb2a.ctex";
    
    protected override bool ShouldGlowGoldInternal => !Osty.CheckMissingWithAnim(Owner);
    
    protected override HashSet<CardTag> CanonicalTags => [ CardTag.OstyAttack ];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => new _003C_003Ez__ReadOnlySingleElementList<CardKeyword>(Extensions.Keywords.DualWield);
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        HoverTipFactory.Static(StaticHoverTip.Channeling),
        HoverTipFactory.FromOrb<GlassOrb>()
    ]);

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new RepeatVar(1),
        new DamageVar(8m, ValueProp.Move),
        new OstyDamageVar(4m, ValueProp.Move)
    ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).WithHitCount(2).FromCard(this)
            .OnlyPlayAnimOnce()
            .Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
        for (int i = 0; i < DynamicVars.Repeat.IntValue; i++)
        {
            await OrbCmd.Channel<GlassOrb>(choiceContext, Owner);
        }
        if (!Osty.CheckMissingWithAnim(Owner) && Owner.Osty != null)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target);
            await DamageCmd.Attack(DynamicVars.OstyDamage.BaseValue).WithHitCount(2).FromOsty(Owner.Osty, this)
                .Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_blunt")
                .Execute(choiceContext);
            for (int i = 0; i < DynamicVars.Repeat.IntValue; i++)
            {
                await OrbCmd.Channel<GlassOrb>(choiceContext, Owner);
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4m);
        DynamicVars.OstyDamage.UpgradeValueBy(2m);
    }
}