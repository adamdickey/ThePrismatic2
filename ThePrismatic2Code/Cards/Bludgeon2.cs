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
using ThePrismatic2.ThePrismatic2Code.Orbs;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Bludgeon2() : ThePrismatic2Card(3, 
    CardType.Attack, CardRarity.Uncommon, 
    TargetType.AnyEnemy)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<IroncladCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/bludgeon.png-46e9e5632a8dbd63cc2066c4317184cd.ctex";
    public override string PortraitPath => "res://.godot/imported/bludgeon.png-46e9e5632a8dbd63cc2066c4317184cd.ctex";
    
    /*
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        HoverTipFactory.Static(StaticHoverTip.Channeling),
        HoverTipFactory.FromOrb<IronOrb>()
    ]);*/

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(Extensions.Keywords.Costly));

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new DamageVar(27m, ValueProp.Move),
        //new DynamicVar("Orbs", 2m)
    ]);
/*
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
            .Execute(choiceContext);
        for (int i = 0; i < DynamicVars["Orbs"].IntValue; i++)
        {
            await OrbCmd.Channel<IronOrb>(choiceContext, Owner);
        }
    }

    */
    public override async Task AfterCardEnteredCombat(CardModel card)
    {
        if (card == this && !IsClone)
        {
            int amount = CombatManager.Instance.History.CardPlaysFinished.Count(e => e.CardPlay.Resources.EnergyValue + Math.Max(0, e.CardPlay.Resources.StarValue) >= 2 && e.CardPlay.Card.Owner == Owner && e.HappenedThisTurn(CombatState));
            ReduceCostBy(amount);
        }
    }

    public override async Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner == Owner && cardPlay.Card.EnergyCost.GetResolved() + Math.Max(0, cardPlay.Card.CurrentStarCost) >= 2)
        {
            ReduceCostBy(1);
        }
    }

    private void ReduceCostBy(int amount)
    {
        EnergyCost.AddThisTurn(-amount);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(8m);
    }
}