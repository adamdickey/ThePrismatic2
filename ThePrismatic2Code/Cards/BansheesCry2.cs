using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class BansheesCry2() : ThePrismatic2Card(11, 
    CardType.Attack, CardRarity.Rare, 
    TargetType.AllEnemies)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<NecrobinderCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/banshees_cry.png-5685018fb8babe6b829128677b3b1ba0.ctex";
    public override string PortraitPath => "res://.godot/imported/banshees_cry.png-5685018fb8babe6b829128677b3b1ba0.ctex";
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        HoverTipFactory.FromKeyword(CardKeyword.Ethereal),
        HoverTipFactory.FromKeyword(Extensions.Keywords.Costly)
        ]);

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new DamageVar(33m, ValueProp.Move),
        new EnergyVar(2)
    ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState != null)
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).TargetingAllOpponents(CombatState)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-2);
    }

    public override Task AfterCardEnteredCombat(CardModel card)
    {
        if (card != this)
        {
            return Task.CompletedTask;
        }
        if (IsClone)
        {
            return Task.CompletedTask;
        }
        int num = CombatManager.Instance.History.CardPlaysFinished.Count(e => (e.WasEthereal || e.CardPlay.Card.EnergyCost.GetResolved() + Math.Max(0, e.CardPlay.Card.LastStarsSpent) >= 2) && e.CardPlay.Card.Owner == Owner);
        EnergyCost.AddThisCombat(-num * DynamicVars.Energy.IntValue);
        return Task.CompletedTask;
    }

    public override Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner)
        {
            return Task.CompletedTask;
        }
        if (!cardPlay.Card.Keywords.Contains(CardKeyword.Ethereal) && !(cardPlay.Card.EnergyCost.GetResolved() + cardPlay.Card.LastStarsSpent >= 2))
        {
            return Task.CompletedTask;
        }
        EnergyCost.AddThisCombat(-DynamicVars.Energy.IntValue);
        return Task.CompletedTask;
    }
}