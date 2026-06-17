using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Orbs;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Tempest2() : ThePrismatic2Card(0, 
    CardType.Skill, CardRarity.Uncommon, 
    TargetType.Self)
{
    public override string CustomPortraitPath => "res://.godot/imported/tempest.png-e431f075eccfa9a2cf6acc592ae16e2a.ctex";
    public override string PortraitPath => "res://.godot/imported/tempest.png-e431f075eccfa9a2cf6acc592ae16e2a.ctex";
    
    protected override bool HasEnergyCostX => true;
    public override bool HasStarCostX => true;
    

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        HoverTipFactory.Static(StaticHoverTip.Channeling),
        HoverTipFactory.FromOrb<LightningOrb>()
        ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        int numOfOrbs = ResolveEnergyXValue() + ResolveStarXValue();
        if (IsUpgraded)
        {
            numOfOrbs++;
        }
        for (int i = 0; i < numOfOrbs; i++)
        {
            await OrbCmd.Channel<LightningOrb>(choiceContext, Owner);
        }
    }
}