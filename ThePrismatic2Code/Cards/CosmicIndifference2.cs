using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class CosmicIndifference2() : ThePrismatic2Card(1, 
    CardType.Skill, CardRarity.Common, 
    TargetType.Self)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<RegentCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/cosmic_indifference.png-3197717bd9f3863c4c469f0216529970.ctex";
    public override string PortraitPath => "res://.godot/imported/cosmic_indifference.png-3197717bd9f3863c4c469f0216529970.ctex";
    
    public override bool GainsBlock => true;

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new BlockVar(5m, ValueProp.Move));

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        CardSelectorPrefs prefs = new CardSelectorPrefs(SelectionScreenPrompt, 1);
        PileType.Discard.GetPile(Owner);
        CardModel? cardModel = (await CardSelectCmd.FromCombatPile(choiceContext, PileType.Discard.GetPile(Owner), Owner, prefs)).FirstOrDefault();
        bool flag = cardModel != null;
        bool flag2 = flag;
        if (flag2)
        {
            bool flag3;
            switch (cardModel?.Pile?.Type)
            {
                case PileType.Draw:
                case PileType.Discard:
                    flag3 = true;
                    break;
                default:
                    flag3 = false;
                    break;
            }
            flag2 = flag3;
        }
        if (flag2 && cardModel != null)
        {
            CardCmd.Upgrade(cardModel);
            await CardPileCmd.Add(cardModel, PileType.Draw, CardPilePosition.Top);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3m);
    }
}