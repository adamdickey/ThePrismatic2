using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using ThePrismatic2.ThePrismatic2Code.Cards;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class CrushUnder2Power : ThePrismatic2Power
{
	public override string CustomPackedIconPath => "res://.godot/imported/crush_under_power.png-b22154f78702d420a2c89c7975576f62.s3tc.ctex";

	public override string CustomBigIconPath => "res://.godot/imported/crush_under_power.png-b22154f78702d420a2c89c7975576f62.s3tc.ctex";

	protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<CrushUnder2>());

	private bool _shouldIgnoreNextInstance;

	public override PowerType Type
	{
		get
		{
			if (!IsPositive)
			{
				return PowerType.Debuff;
			}
			return PowerType.Buff;
		}
	}

	public override PowerStackType StackType => PowerStackType.Counter;

	protected bool IsPositive => false;

	private int Sign
	{
		get
		{
			if (!IsPositive)
			{
				return -1;
			}
			return 1;
		}
	}

	public override async Task BeforeApplied(Creature target, decimal amount, Creature? applier, CardModel? cardSource)
	{
		if (_shouldIgnoreNextInstance)
		{
			_shouldIgnoreNextInstance = false;
		}
		else
		{
			await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), target, Sign * amount, applier, cardSource, silent: true);
		}
	}

	public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
	{
		if (amount != Amount && power == this)
		{
			if (_shouldIgnoreNextInstance)
			{
				_shouldIgnoreNextInstance = false;
			}
			else
			{
				await PowerCmd.Apply<StrengthPower>(choiceContext, Owner, Sign * amount, applier, cardSource, silent: true);
			}
		}
	}

	public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
	{
		if (side == Owner.Side)
		{
			Flash();
			await PowerCmd.Remove(this);
			await PowerCmd.Apply<StrengthPower>(choiceContext, Owner, -Sign * Amount, Owner, null);
		}
	}
}
