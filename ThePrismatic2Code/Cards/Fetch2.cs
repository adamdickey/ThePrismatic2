using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Fetch2() : ThePrismatic2Card(0, 
    CardType.Attack, CardRarity.Uncommon, 
    TargetType.AnyEnemy)
{
    public override string CustomPortraitPath => "res://.godot/imported/fetch.png-0612438a7c52981fe0822030793632ab.ctex";
    public override string PortraitPath => "res://.godot/imported/fetch.png-0612438a7c52981fe0822030793632ab.ctex";
    
    protected override HashSet<CardTag> CanonicalTags => [CardTag.OstyAttack];

    protected override bool ShouldGlowGoldInternal => !HasBeenPlayedThisTurn;

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new SummonVar(1m),
        new OstyDamageVar(2m, ValueProp.Move),
        new CardsVar(1)
    ]);

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        HoverTipFactory.Static(StaticHoverTip.SummonDynamic, DynamicVars.Summon),
        HoverTipFactory.FromKeyword(Extensions.Keywords.Costly)
        ]);

    private bool HasBeenPlayedThisTurn => CombatManager.Instance.History.CardPlaysFinished.Any(e => e.CardPlay.Card == this && e.HappenedThisTurn(CombatState));

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await OstyCmd.Summon(choiceContext, Owner, DynamicVars.Summon.BaseValue, this);
        if (!Osty.CheckMissingWithAnim(Owner) && Owner.Osty != null)
        {
            await DamageCmd.Attack(DynamicVars.OstyDamage.BaseValue).FromOsty(Owner.Osty, this).Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
                .Execute(choiceContext);
            if (!HasBeenPlayedThisTurn)
            {
                await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.OstyDamage.UpgradeValueBy(2m);
    }
}