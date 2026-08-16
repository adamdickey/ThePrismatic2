using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Thrash2() : ThePrismatic2Card(1, 
    CardType.Attack, CardRarity.Rare, 
    TargetType.AnyEnemy)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<IroncladCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/thrash.png-cf3e2f34b99087c931de4b5b1a14c3c8.ctex";
    public override string PortraitPath => "res://.godot/imported/thrash.png-cf3e2f34b99087c931de4b5b1a14c3c8.ctex";
    
    private decimal _extraDamage;

	protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Exhaust));

	protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new DamageVar(4m, ValueProp.Move));

	private decimal ExtraDamage
	{
		get
		{
			return _extraDamage;
		}
		set
		{
			AssertMutable();
			_extraDamage = value;
		}
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(cardPlay.Target);
		await DamageCmd.Attack(DynamicVars.Damage.BaseValue).WithHitCount(2).FromCard(this)
			.Targeting(cardPlay.Target)
			.WithHitFx("vfx/vfx_thrash")
			.Execute(choiceContext);
		CardPile pile = PileType.Hand.GetPile(Owner);
		CardModel? cardModel = Owner.RunState.Rng.CombatCardSelection.NextItem(pile.Cards.Where(c => c.Type == CardType.Attack));
		if (cardModel != null)
		{
			decimal damage = default(decimal);
			if (cardModel.DynamicVars.ContainsKey("CalculatedDamage"))
			{
				damage = cardModel.DynamicVars.CalculatedDamage.Calculate(null);
			}
			else if (cardModel.DynamicVars.ContainsKey("Damage"))
			{
				damage = cardModel.DynamicVars.Damage.BaseValue;
			}
			else if (cardModel.DynamicVars.ContainsKey("OstyDamage"))
			{
				damage = cardModel.DynamicVars.OstyDamage.BaseValue;
			}
			else
			{
				Log.Warn(Id.Entry + " exhausted attack card " + cardModel.Id.Entry + " that did not have an appropriate damage var!");
			}
			damage = Hook.ModifyDamage(Owner.RunState, Owner.Creature.CombatState, null, Owner.Creature, damage, ValueProp.Move, cardModel, ModifyDamageHookType.All, CardPreviewMode.None, out IEnumerable<AbstractModel> _);
			DynamicVars.Damage.BaseValue += damage;
			ExtraDamage += damage;
			await CardCmd.Exhaust(choiceContext, cardModel);
		}
	}

	protected override void AfterDowngraded()
	{
		base.AfterDowngraded();
		DynamicVars.Damage.BaseValue += ExtraDamage;
	}

	protected override void OnUpgrade()
	{
		DynamicVars.Damage.UpgradeValueBy(2m);
	}
}