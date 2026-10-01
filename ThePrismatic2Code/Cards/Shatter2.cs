using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

public class Shatter2() : ThePrismatic2Card(1, 
    CardType.Attack, CardRarity.Rare, 
    TargetType.AllEnemies)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<DefectCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/shatter.png-429cb605a1a09eb961a06ffddda3e28d.ctex";
    public override string PortraitPath => "res://.godot/imported/shatter.png-429cb605a1a09eb961a06ffddda3e28d.ctex";
    
    public override OrbEvokeType OrbEvokeType => OrbEvokeType.All;
    
    protected override bool ShouldGlowGoldInternal => !Osty.CheckMissingWithAnim(Owner);
    protected override HashSet<CardTag> CanonicalTags => [CardTag.OstyAttack];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => new _003C_003Ez__ReadOnlyArray<CardKeyword>([
        CardKeyword.Exhaust,
        Extensions.Keywords.DualWield
    ]);

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Evoke));

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new DamageVar(6m, ValueProp.Move),
        new OstyDamageVar(3m, ValueProp.Move)
    ]);

    private bool _ostyPlay;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState != null)
            if (_ostyPlay && Owner.Osty != null)
            {
                await DamageCmd.Attack(DynamicVars.OstyDamage.BaseValue).FromOsty(Owner.Osty, this)
                    .TargetingAllOpponents(CombatState)
                    .WithHitFx("vfx/vfx_attack_slash")
                    .Execute(choiceContext);
            }
            else
            {
                await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this)
                    .TargetingAllOpponents(CombatState)
                    .WithHitFx("vfx/vfx_attack_slash")
                    .Execute(choiceContext);  
            }
        if (Owner.PlayerCombatState != null)
        {
            int orbCount = Owner.PlayerCombatState.OrbQueue.Orbs.Count;
            for (int i = 0; i < orbCount; i++)
            {
                await OrbCmd.EvokeNext(choiceContext, Owner, dequeue: false);
                await OrbCmd.EvokeNext(choiceContext, Owner, dequeue: _ostyPlay || Osty.CheckMissingWithAnim(Owner));
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4m);
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