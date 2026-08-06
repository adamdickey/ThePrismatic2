using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Characters;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class PullAggro2() : ThePrismatic2Card(2, 
    CardType.Skill, CardRarity.Common, 
    TargetType.Self)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<NecrobinderCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/pull_aggro.png-45fa9c15b184e40c135d8c222daa8453.ctex";
    public override string PortraitPath => "res://.godot/imported/pull_aggro.png-45fa9c15b184e40c135d8c222daa8453.ctex";
    
    public override bool GainsBlock => true;

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new SummonVar(4m),
        new CardsVar(1)
    ]);

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        HoverTipFactory.Static(StaticHoverTip.SummonDynamic, DynamicVars.Summon),
        HoverTipFactory.FromCard<MinionSacrifice>(IsUpgraded)
        ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, Necrobinder.GetSummonAnimIfApplicable(Owner.Character), Necrobinder.GetSummonDelayIfApplicable(Owner.Character));
        await OstyCmd.Summon(choiceContext, Owner, DynamicVars.Summon.BaseValue, this);
        CardModel? cardModel = CombatState?.CreateCard<MinionSacrifice>(Owner);
        if (cardModel != null)
        {
            if (IsUpgraded)
            {
                CardCmd.Upgrade(cardModel);
            }
            await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Hand, Owner);
        }
    }
}