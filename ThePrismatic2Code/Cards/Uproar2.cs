using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Uproar2() : ThePrismatic2Card(2, 
    CardType.Attack, CardRarity.Common, 
    TargetType.AnyEnemy)
{
    public override string CustomPortraitPath => "res://.godot/imported/uproar.png-a8bc36a119474d14dc3e3bbac995f2a0.ctex";
    public override string PortraitPath => "res://.godot/imported/uproar.png-a8bc36a119474d14dc3e3bbac995f2a0.ctex";
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => new _003C_003Ez__ReadOnlySingleElementList<CardKeyword>(Extensions.Keywords.DualWield);
    
    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new DamageVar(2m, ValueProp.Move));
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
    	ArgumentNullException.ThrowIfNull(cardPlay.Target);
    	await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).WithHitCount(2)
    		.Targeting(cardPlay.Target)
    		.WithHitFx("vfx/vfx_attack_slash")
    		.Execute(choiceContext);
    	CardModel? cardModel = PileType.Draw.GetPile(Owner).Cards.Where(c => c.Type == CardType.Attack && !c.Keywords.Contains(CardKeyword.Unplayable)).ToList().StableShuffle(Owner.RunState.Rng.Shuffle).FirstOrDefault();
    	if (cardModel == null)
    	{
    		cardModel = PileType.Draw.GetPile(Owner).Cards.Where(c => c.Type == CardType.Attack).ToList().StableShuffle(Owner.RunState.Rng.Shuffle).FirstOrDefault();
    	}
    	if (cardModel != null)
    	{
    		await CardCmd.AutoPlay(choiceContext, cardModel, null);
    	}
	    if (!Osty.CheckMissingWithAnim(Owner) && Owner.Osty != null)
	    {
		    ArgumentNullException.ThrowIfNull(cardPlay.Target);
		    await DamageCmd.Attack(DynamicVars.Damage.BaseValue/2).FromOsty(Owner.Osty, this).WithHitCount(2)
			    .Targeting(cardPlay.Target)
			    .WithHitFx("vfx/vfx_attack_slash")
			    .Execute(choiceContext);
		    cardModel = PileType.Draw.GetPile(Owner).Cards.Where(c => c.Type == CardType.Attack && !c.Keywords.Contains(CardKeyword.Unplayable)).ToList().StableShuffle(Owner.RunState.Rng.Shuffle).FirstOrDefault();
		    if (cardModel == null)
		    {
			    cardModel = PileType.Draw.GetPile(Owner).Cards.Where(c => c.Type == CardType.Attack).ToList().StableShuffle(Owner.RunState.Rng.Shuffle).FirstOrDefault();
		    }
		    if (cardModel != null)
		    {
			    await CardCmd.AutoPlay(choiceContext, cardModel, null);
		    }
	    }
    }

    protected override void OnUpgrade()
    {
    	DynamicVars.Damage.UpgradeValueBy(2m);
    }
}