using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class BundleOfJoy2() : ThePrismatic2Card(1, 
    CardType.Skill, CardRarity.Rare, 
    TargetType.Self)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<RegentCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/bundle_of_joy.png-cb7480ef183d6f8d11c2f226481e96e2.ctex";
    public override string PortraitPath => "res://.godot/imported/bundle_of_joy.png-cb7480ef183d6f8d11c2f226481e96e2.ctex";

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new PowerVar<FocusPower>(1),
        new CardsVar(3)
        ]);

    public override IEnumerable<CardKeyword> CanonicalKeywords => new _003C_003Ez__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<FocusPower>());

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (RunState != null)
        {
            await PowerCmd.Apply<FocusPower>(choiceContext, Owner.Creature, DynamicVars["FocusPower"].BaseValue, Owner.Creature, this);
            IEnumerable<CardModel> distinctForCombat = CardFactory.GetDistinctForCombat(Owner, ModelDb.CardPool<ColorlessCardPool>().GetUnlockedCards(Owner.UnlockState, RunState.CardMultiplayerConstraint), DynamicVars.Cards.IntValue, Owner.RunState.Rng.CombatCardGeneration);
            foreach (CardModel item in distinctForCombat)
            {
                await CardPileCmd.AddGeneratedCardToCombat(item, PileType.Hand, Owner);
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1m);
    }
}