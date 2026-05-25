using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class SetupStrike2Power : ThePrismatic2Power
{
    
    public override string CustomPackedIconPath => "res://.godot/imported/setup_strike_power.png-e2066869b50711336c6583b0d0f8f63f.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/setup_strike_power.png-e2066869b50711336c6583b0d0f8f63f.s3tc.ctex";
    
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

	public PowerModel InternallyAppliedPower => ModelDb.Power<StrengthPower>();

	protected virtual bool IsPositive => true;

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

	public void IgnoreNextInstance()
	{
		_shouldIgnoreNextInstance = true;
	}

	public override async Task BeforeApplied(Creature target, decimal amount, Creature? applier, CardModel? cardSource)
	{
		if (_shouldIgnoreNextInstance)
		{
			_shouldIgnoreNextInstance = false;
		}
		else
		{
			await PowerCmd.Apply<StrengthPower>(target, (decimal)Sign * amount, applier, cardSource, silent: true);
			await PowerCmd.Apply<CalcifyPower>(target, (decimal)Sign * amount, applier, cardSource, silent: true);
		}
	}

	public override async Task AfterPowerAmountChanged(PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
	{
		if (!(amount == (decimal)Amount) && power == this)
		{
			if (_shouldIgnoreNextInstance)
			{
				_shouldIgnoreNextInstance = false;
			}
			else
			{
				await PowerCmd.Apply<StrengthPower>(Owner, (decimal)Sign * amount, applier, cardSource, silent: true);
				await PowerCmd.Apply<CalcifyPower>(Owner, (decimal)Sign * amount, applier, cardSource, silent: true);
			}
		}
	}

	public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
	{
		if (side == Owner.Side)
		{
			Flash();
			await PowerCmd.Remove(this);
			await PowerCmd.Apply<StrengthPower>(Owner, -Sign * Amount, Owner, null);
			await PowerCmd.Apply<CalcifyPower>(Owner, -Sign * Amount, Owner, null);
		}
	}
}
