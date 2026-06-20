using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class Oblivion2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/oblivion_power.png-b6fbbf4df2421efbfd5b8d85d8910ea8.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/oblivion_power.png-b6fbbf4df2421efbfd5b8d85d8910ea8.s3tc.ctex";
    
    private class Data
	{
		public readonly Dictionary<CardModel, int> AmountsForPlayedCards = new();
	}
    
    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new PowerVar<DoomPower>(0m));

    protected override object InitInternalData()
    {
    	return new Data();
    }
    
    public override Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
	    if (power == this)
	    {
		    DynamicVars.Doom.UpgradeValueBy(2m);
	    }
	    return Task.CompletedTask;
    }

    public override Task BeforeCardPlayed(CardPlay cardPlay)
    {
    	if (Applier?.Player == null)
    	{
    		return Task.CompletedTask;
    	}
    	if (cardPlay.Card.Owner != Applier.Player)
    	{
    		return Task.CompletedTask;
    	}
    	GetInternalData<Data>().AmountsForPlayedCards.Add(cardPlay.Card, Amount);
    	return Task.CompletedTask;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
    	if (GetInternalData<Data>().AmountsForPlayedCards.Remove(cardPlay.Card, out var value))
    	{
    		Flash();
    		await PowerCmd.Apply<PoisonPower>(context, Owner, value, Applier, null);
			await PowerCmd.Apply<DoomPower>(context, Owner, DynamicVars.Doom.BaseValue, Applier, null);
    	}
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
    	if (side == CombatSide.Player)
    	{
    		await PowerCmd.Remove(this);
    	}
    }
}
