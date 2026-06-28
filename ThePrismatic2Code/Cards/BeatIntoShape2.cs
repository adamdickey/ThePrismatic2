using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
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
public class BeatIntoShape2() : ThePrismatic2Card(1, 
    CardType.Attack, CardRarity.Rare, 
    TargetType.AnyEnemy)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<RegentCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/beat_into_shape.png-99c90e2e819d29996ecfc0436e19889c.ctex";
    public override string PortraitPath => "res://.godot/imported/beat_into_shape.png-99c90e2e819d29996ecfc0436e19889c.ctex";

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new DamageVar(5m, ValueProp.Move),
        new CalculationBaseVar(1m),
        new CalculationExtraVar(1m),
        new CalculatedVar("CalculatedOrbs").WithMultiplier((card, target) => CombatManager.Instance.History.Entries.OfType<DamageReceivedEntry>().Count(e => e.Receiver == target && e.Dealer == card.Owner.Creature && e.Result.Props.IsPoweredAttack() && e.HappenedThisTurn(card.CombatState)))
    ]);
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            List<IHoverTip> list =
            [
                HoverTipFactory.Static(StaticHoverTip.Channeling),
                HoverTipFactory.FromOrb<IronOrb>()
            ];
            list.AddRange(HoverTipFactory.FromForge());
            return new _003C_003Ez__ReadOnlyList<IHoverTip>(list);
        }
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        AttackCommand attackCommand = await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
            .Execute(choiceContext);
        decimal amount = ((CalculatedVar)DynamicVars["CalculatedOrbs"]).Calculate(cardPlay.Target);
        amount -= attackCommand.Results.Count() * DynamicVars.CalculationExtra.BaseValue;
        for (int i = 0; i < amount; i++)
        {
            await OrbCmd.Channel<IronOrb>(choiceContext, Owner);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
    }
}