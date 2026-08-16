using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

public class Severance2() : ThePrismatic2Card(2, 
    CardType.Attack, CardRarity.Uncommon, 
    TargetType.AnyEnemy)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<NecrobinderCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/severance.png-4b757b8f9d472e60d56f80536dd3a946.ctex";
    public override string PortraitPath => "res://.godot/imported/severance.png-4b757b8f9d472e60d56f80536dd3a946.ctex";
    
    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new DamageVar(13m, ValueProp.Move));

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<Soul>());

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
        if (CombatState != null)
        {
            List<Soul> souls = Soul.Create(Owner, 3, CombatState).ToList();
            CardPileAddResult drawResult = await CardPileCmd.AddGeneratedCardToCombat(souls[0], PileType.Draw, Owner, CardPilePosition.Random);
            CardPileAddResult discardResult = await CardPileCmd.AddGeneratedCardToCombat(souls[1], PileType.Discard, Owner);
            await CardPileCmd.AddGeneratedCardToCombat(souls[2], PileType.Hand, Owner);
            CardCmd.PreviewCardPileAdd(new _003C_003Ez__ReadOnlyArray<CardPileAddResult>([drawResult, discardResult]));
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(5m);
    }
}