using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Flechettes2() : ThePrismatic2Card(1, 
    CardType.Attack, CardRarity.Uncommon, 
    TargetType.AnyEnemy)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<SilentCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/flechettes.png-ae379652f6f8ac6ac8d04f2f88c0f9a6.ctex";
    public override string PortraitPath => "res://.godot/imported/flechettes.png-ae379652f6f8ac6ac8d04f2f88c0f9a6.ctex";
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<DoomPower>());

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new DamageVar(3m, ValueProp.Move),
        new PowerVar<DoomPower>(3m),
        new CalculationBaseVar(0m),
        new CalculationExtraVar(1m),
        new CalculatedVar("CalculatedHits").WithMultiplier((card, _) => PileType.Hand.GetPile(card.Owner).Cards.Count(c => c.Type == CardType.Skill))
    ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        for (int i = 0; i < (int)((CalculatedVar)DynamicVars["CalculatedHits"]).Calculate(cardPlay.Target); i++)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this)
                .Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash", null, "dagger_throw.mp3")
                .Execute(choiceContext);
            await PowerCmd.Apply<DoomPower>(choiceContext, cardPlay.Target, DynamicVars.Doom.BaseValue, Owner.Creature, this);
        } 
        //await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).WithHitCount((int)((CalculatedVar)DynamicVars["CalculatedHits"]).Calculate(cardPlay.Target)).FromCard(this)
        //    .Targeting(cardPlay.Target)
        //    .WithHitFx("vfx/vfx_attack_slash", null, "dagger_throw.mp3")
        //    .Execute(choiceContext);
        //await PowerCmd.Apply<DoomPower>(choiceContext, cardPlay.Target, DynamicVars.Doom.BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1m);
        DynamicVars.Doom.UpgradeValueBy(1m);
    }
}