using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class Monologue2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/monologue_power.png-096c21014156ee74d576f110f38531de.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/monologue_power.png-096c21014156ee74d576f110f38531de.s3tc.ctex";

    private int _cardsPlayed;
    private class Data
	{
		/// <summary>
		/// Keep track of the cards we've seen played and the power amount at the time they were played.
		/// This lets Monologue avoid triggering on cards that started play before it was applied, and avoid gaining
		/// extra block on multiple plays of Monologue.
		/// </summary>
		public readonly Dictionary<CardModel, int> AmountsForPlayedCards = new();
	}

	public override PowerType Type => PowerType.Buff;

	public override PowerStackType StackType
	{
		get
		{
			if (DynamicVars["StrengthApplied"].IntValue != 0)
			{
				return PowerStackType.Counter;
			}
			return PowerStackType.None;
		}
	}

	public override int DisplayAmount => DynamicVars["StrengthApplied"].IntValue;

	public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

	protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
		new PowerVar<StrengthPower>(1m),
		new PowerVar<FocusPower>(1m),
		new DynamicVar("StrengthApplied", 0m),
		new DynamicVar("FocusApplied", 0m)
	]);

	protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
		HoverTipFactory.FromPower<StrengthPower>(),
		HoverTipFactory.FromPower<FocusPower>()
	]);

	protected override object InitInternalData()
	{
		return new Data();
	}

	public override Task BeforeCardPlayed(CardPlay cardPlay)
	{
		if (cardPlay.Card.Owner.Creature != Owner)
		{
			return Task.CompletedTask;
		}
		GetInternalData<Data>().AmountsForPlayedCards.Add(cardPlay.Card, DynamicVars.Strength.IntValue);
		return Task.CompletedTask;
	}

	public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		if (cardPlay.Card.Owner == Owner.Player && GetInternalData<Data>().AmountsForPlayedCards.Remove(cardPlay.Card, out var value))
		{
			_cardsPlayed += 1;
			if (_cardsPlayed >= 2)
			{
				_cardsPlayed = 0;
				Flash();
				await PowerCmd.Apply<StrengthPower>(choiceContext, Owner, value, Owner, null, silent: true);
				await PowerCmd.Apply<FocusPower>(choiceContext, Owner, value, Owner, null, silent: true);
				DynamicVars["StrengthApplied"].BaseValue += DynamicVars.Strength.IntValue;
				DynamicVars["FocusApplied"].BaseValue += DynamicVars["FocusPower"].IntValue;
				InvokeDisplayAmountChanged();
			}
		}
	}

	public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
	{
		if (participants.Contains(Owner))
		{
			await PowerCmd.Remove(this);
			await PowerCmd.Apply<StrengthPower>(choiceContext, Owner, -DynamicVars["StrengthApplied"].BaseValue, Owner, null, silent: true);
			await PowerCmd.Apply<FocusPower>(choiceContext, Owner, -DynamicVars["FocusApplied"].BaseValue, Owner, null, silent: true);
		}
	}
}
