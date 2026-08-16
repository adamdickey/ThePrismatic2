using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

public class Feral2() : ThePrismatic2Card(2, 
    CardType.Power, CardRarity.Uncommon, 
    TargetType.Self)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<DefectCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/feral.png-b960b359f4f735195279b0704bb55598.ctex";
    public override string PortraitPath => "res://.godot/imported/feral.png-b960b359f4f735195279b0704bb55598.ctex";
    
    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new PowerVar<FeralPower>(1m));

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
	    await CreatureCmd.TriggerAnim(Owner.Creature, "PowerUp", Owner.Character.PowerUpAnimDelay);
	    await PowerCmd.Apply<FeralPower>(choiceContext, Owner.Creature, DynamicVars["FeralPower"].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
	    EnergyCost.UpgradeBy(-1);
    }
}