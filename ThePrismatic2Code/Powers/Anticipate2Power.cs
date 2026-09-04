using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using ThePrismatic2.ThePrismatic2Code.Cards;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class Anticipate2Power : ThePrismatic2Power
{
	public override string CustomPackedIconPath => "res://.godot/imported/anticipate_power.png-f04687be18ba16beaa4d734d20d91615.s3tc.ctex";
	public override string CustomBigIconPath => "res://.godot/imported/anticipate_power.png-f04687be18ba16beaa4d734d20d91615.s3tc.ctex";

	protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
		HoverTipFactory.FromPower<DexterityPower>(),
		HoverTipFactory.FromPower<FocusPower>(),
		HoverTipFactory.FromCard<Anticipate2>()
		]);

	protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new PowerVar<FocusPower>(0m));

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

	public override async Task BeforeApplied(Creature target, decimal amount, Creature? applier, CardModel? cardSource)
	{
		if (_shouldIgnoreNextInstance)
		{
			_shouldIgnoreNextInstance = false;
		}
		else
		{
			DynamicVars["FocusPower"].BaseValue++;
			await PowerCmd.Apply<DexterityPower>(new ThrowingPlayerChoiceContext(), target, Sign * amount, applier, cardSource, silent: true);
			await PowerCmd.Apply<FocusPower>(new ThrowingPlayerChoiceContext(), target, Sign * 1, applier, cardSource, silent: true);
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
				DynamicVars["FocusPower"].BaseValue++;
				await PowerCmd.Apply<DexterityPower>(choiceContext, Owner, Sign * amount, applier, cardSource, silent: true);
				await PowerCmd.Apply<FocusPower>(choiceContext, Owner, Sign * 1, applier, cardSource, silent: true);
			}
		}
	}

	public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
	{
		if (side == Owner.Side)
		{
			Flash();
			await PowerCmd.Remove(this);
			await PowerCmd.Apply<DexterityPower>(choiceContext, Owner, -Sign * Amount, Owner, null);
			await PowerCmd.Apply<FocusPower>(choiceContext, Owner, -Sign * DynamicVars["FocusPower"].BaseValue, Owner, null);
		}
	}
}
