using BaseLib.Utils;
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
public class ParticleWall2() : ThePrismatic2Card(0, 
    CardType.Skill, CardRarity.Uncommon, 
    TargetType.Self)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<RegentCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/particle_wall.png-9674eb2b7f59122adcff3b2183c1b499.ctex";
    public override string PortraitPath => "res://.godot/imported/particle_wall.png-9674eb2b7f59122adcff3b2183c1b499.ctex";

    public override int CanonicalStarCost => 2;

    public override bool GainsBlock => true;
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => new _003C_003Ez__ReadOnlySingleElementList<CardKeyword>(Extensions.Keywords.Starbound);
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.SummonDynamic, DynamicVars.Summon));

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new BlockVar(6m, ValueProp.Move),
        new SummonVar(2m)
    ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        await OstyCmd.Summon(choiceContext, Owner, DynamicVars.Summon.BaseValue, this);
        await Cmd.Wait(0.25f);
    }

    protected override PileType GetResultPileTypeForCardPlay()
    {
        PileType resultPileTypeForCardPlay = base.GetResultPileTypeForCardPlay();
        if (resultPileTypeForCardPlay != PileType.Discard)
        {
            return resultPileTypeForCardPlay;
        }
        return PileType.Hand;
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(2m);
        DynamicVars.Summon.UpgradeValueBy(1m);
    }
}