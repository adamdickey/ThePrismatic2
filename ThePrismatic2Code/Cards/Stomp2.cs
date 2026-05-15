using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;
using ThePrismatic2.ThePrismatic2Code.Powers;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Stomp2() : ThePrismatic2Card(3, 
    CardType.Attack, CardRarity.Uncommon, 
    TargetType.AllEnemies)
{
    public override string CustomPortraitPath => "res://.godot/imported/stomp.png-9aac0da66ce194d4ae3678059f67ad08.ctex";
    public override string PortraitPath => "res://.godot/imported/stomp.png-9aac0da66ce194d4ae3678059f67ad08.ctex";

    protected override IEnumerable<DynamicVar> CanonicalVars => new global::_003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new DamageVar(12m, ValueProp.Move));
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new global::_003C_003Ez__ReadOnlyArray<IHoverTip>(new IHoverTip[2]
    {
        HoverTipFactory.Static(StaticHoverTip.Channeling),
        HoverTipFactory.FromOrb<GlassOrb>()
    });

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.AttackAnimDelay);
        foreach (Creature hittableEnemy in base.CombatState.HittableEnemies)
        {
            NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(NSpikeSplashVfx.Create(hittableEnemy));
        }
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).TargetingAllOpponents(base.CombatState)
            .WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
            .Execute(choiceContext);
        await OrbCmd.Channel<GlassOrb>(choiceContext, base.Owner);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(3m);
    }

    public override Task AfterCardEnteredCombat(CardModel card)
    {
        if (card != this)
        {
            return Task.CompletedTask;
        }
        if (base.IsClone)
        {
            return Task.CompletedTask;
        }
        int amount = CombatManager.Instance.History.CardPlaysFinished.Count((CardPlayFinishedEntry e) => e.CardPlay.Card.Type == CardType.Attack && e.CardPlay.Card.Owner == base.Owner && e.HappenedThisTurn(base.CombatState));
        ReduceCostBy(amount);
        return Task.CompletedTask;
    }

    public override Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != base.Owner)
        {
            return Task.CompletedTask;
        }
        if (cardPlay.Card.Type != CardType.Attack)
        {
            return Task.CompletedTask;
        }
        ReduceCostBy(1);
        return Task.CompletedTask;
    }

    private void ReduceCostBy(int amount)
    {
        base.EnergyCost.AddThisTurn(-amount);
    }
}