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
public class Alignment2() : ThePrismatic2Card(0, 
    CardType.Skill, CardRarity.Uncommon, 
    TargetType.Self)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<RegentCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/alignment.png-a063f4b99717db2f1a30064273e7c39f.ctex";
    public override string PortraitPath => "res://.godot/imported/alignment.png-a063f4b99717db2f1a30064273e7c39f.ctex";
    
    public override int CanonicalStarCost => 2;
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => new _003C_003Ez__ReadOnlySingleElementList<CardKeyword>(Extensions.Keywords.Starbound);
    
    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new StarsVar(1));

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(EnergyHoverTip);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        int energySpent = cardPlay.Card.EnergyCost.GetResolved();
        int starsSpent = cardPlay.Card.LastStarsSpent;
        await PlayerCmd.GainEnergy(starsSpent, Owner);
        await PlayerCmd.GainStars(DynamicVars.Stars.BaseValue*energySpent, Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Stars.UpgradeValueBy(1m);
    }
}