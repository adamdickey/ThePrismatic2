using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class DrumOfBattle2() : ThePrismatic2Card(1, 
    CardType.Skill, CardRarity.Uncommon, 
    TargetType.Self)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<IroncladCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/drum_of_battle.png-17d414e562deb6a6057b9a9cafdde247.ctex";
    public override string PortraitPath => "res://.godot/imported/drum_of_battle.png-17d414e562deb6a6057b9a9cafdde247.ctex";
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Exhaust));

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new CardsVar(2),
        new EnergyVar(2)
    ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Energy.UpgradeValueBy(1m);
    }
    
    public override async Task AfterCardDiscarded(PlayerChoiceContext choiceContext, CardModel card)
    {
        if (card == this)
        {
            await CardCmd.Exhaust(choiceContext, this);
        }
    }
    
    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
    {
        if (card == this)
        {
            await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
        }
    }
}