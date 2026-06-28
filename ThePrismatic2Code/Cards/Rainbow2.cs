using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Orbs;
using ThePrismatic2.ThePrismatic2Code.Character;
using ThePrismatic2.ThePrismatic2Code.Orbs;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Rainbow2() : ThePrismatic2Card(3, 
    CardType.Skill, CardRarity.Rare, 
    TargetType.Self)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<DefectCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/rainbow.png-c56801ae4b7b9d21ee600dbaf7253409.ctex";
    public override string PortraitPath => "res://.godot/imported/rainbow.png-c56801ae4b7b9d21ee600dbaf7253409.ctex";
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => new _003C_003Ez__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
    
    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new RepeatVar(3));

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        HoverTipFactory.Static(StaticHoverTip.Channeling),
        HoverTipFactory.FromOrb<MagmaOrb>(),
        HoverTipFactory.FromOrb<SolarOrb>(),
        HoverTipFactory.FromOrb<VenomOrb>(),
        HoverTipFactory.FromOrb<GloomOrb>()
    ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await OrbCmd.AddSlots(Owner, DynamicVars.Repeat.IntValue);
        await OrbCmd.Channel<MagmaOrb>(choiceContext, Owner);
        await OrbCmd.Channel<SolarOrb>(choiceContext, Owner);
        await OrbCmd.Channel<LightningOrb>(choiceContext, Owner);
        await OrbCmd.Channel<VenomOrb>(choiceContext, Owner);
        await OrbCmd.Channel<FrostOrb>(choiceContext, Owner);
        await OrbCmd.Channel<GloomOrb>(choiceContext, Owner);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}