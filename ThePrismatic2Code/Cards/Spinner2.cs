using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using ThePrismatic2.ThePrismatic2Code.Character;
using ThePrismatic2.ThePrismatic2Code.Powers;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Spinner2() : ThePrismatic2Card(1, 
    CardType.Power, CardRarity.Rare, 
    TargetType.Self)
{
    public override string CustomPortraitPath => "res://.godot/imported/spinner.png-f12dee50bccd7aec5b1ea8c1e84b8e4b.ctex";
    public override string PortraitPath => "res://.godot/imported/spinner.png-f12dee50bccd7aec5b1ea8c1e84b8e4b.ctex";
    
    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new PowerVar<SpinnerPower>(1m));
    
    	protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Channeling));
    
    	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    	{
    		await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
    		if (IsUpgraded)
    		{
			    await OrbCmd.Channel(choiceContext, OrbModel.GetRandomOrb(Owner.RunState.Rng.CombatOrbGeneration).ToMutable(), Owner);
    		}
    		await PowerCmd.Apply<Spinner2Power>(Owner.Creature, DynamicVars["SpinnerPower"].BaseValue, Owner.Creature, this);
    	}
}