using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace ThePrismatic2.ThePrismatic2Code.Powers;


public class Corruption2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/corruption_power.png-11d138ae6f08bbf1f5608c82bb178ff9.s3tc.ctex";

    public override string CustomBigIconPath => "res://.godot/imported/corruption_power.png-11d138ae6f08bbf1f5608c82bb178ff9.s3tc.ctex";

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Exhaust));
    private class Data
    {
	    public int SkillsPlayedThisTurn;
    }

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override object InitInternalData()
    {
	    return new Data();
    }

    public override Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
		GetInternalData<Data>().SkillsPlayedThisTurn = CombatManager.Instance.History.CardPlaysFinished.Count(e => e.CardPlay.Card.Owner == Owner.Player && e.CardPlay.Card.Type == CardType.Skill && e.HappenedThisTurn(CombatState));
	    return Task.CompletedTask;
    }
	public override bool TryModifyEnergyCostInCombatLate(CardModel card, decimal originalCost, out decimal modifiedCost)
	{
		modifiedCost = originalCost;
		if (ShouldSkip(card))
		{
			return false;
		}
		modifiedCost = 0;
		return true;
	}

	public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		if (cardPlay.Card.Owner.Creature == Owner && !cardPlay.IsAutoPlay && cardPlay.IsLastInSeries && cardPlay.Card.Type == CardType.Skill)
		{
			GetInternalData<Data>().SkillsPlayedThisTurn++;
		}
		return Task.CompletedTask;
	}

	public override Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
	{
		if (!participants.Contains(Owner))
		{
			return Task.CompletedTask;
		}
		GetInternalData<Data>().SkillsPlayedThisTurn = 0;
		return Task.CompletedTask;
	}

	private bool ShouldSkip(CardModel card)
	{
		bool flag = card.Owner.Creature != Owner;
		bool flag2 = flag;
		if (!flag2)
		{
			bool flag3;
			switch (card.Pile?.Type)
			{
			case PileType.Hand:
			case PileType.Play:
				flag3 = true;
				break;
			default:
				flag3 = false;
				break;
			}
			flag2 = !flag3;
		}
		if (card.Type != CardType.Skill)
		{
			return true;
		}
		if (!flag2)
		{
			return GetInternalData<Data>().SkillsPlayedThisTurn >= Amount;
		}
		return true;
	}
	public override (PileType, CardPilePosition) ModifyCardPlayResultPileTypeAndPosition(CardModel card, bool isAutoPlay, ResourceInfo resources, PileType pileType, CardPilePosition position)
	{
		if (card.Owner.Creature != Owner || card.Type != CardType.Skill || GetInternalData<Data>().SkillsPlayedThisTurn >= Amount)
		{
			return (pileType, position);
		}
		if (card.Keywords.Contains(Extensions.Keywords.Cunning) || card.Keywords.Contains(Extensions.Keywords.CunningThisTurn))
		{
			if (CombatManager.Instance.History.Entries.OfType<CardPlayFinishedEntry>().LastOrDefault()?.CardPlay.Card == card)
			{
				return (pileType, position);
			}
		}
		return (PileType.Exhaust, position);
	}
/*
    public override Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        if (Owner.Player == null) return Task.CompletedTask;
        IEnumerable<CardModel> enumerable = Owner.Player?.PlayerCombatState?.AllCards ?? Array.Empty<CardModel>();
        foreach (CardModel card in enumerable)
        {
            if (card.Type != CardType.Skill) continue;
            card.EnergyCost.AddThisCombat(-1);
        }
        return Task.CompletedTask;
    }
    
    public override Task AfterCardEnteredCombat(CardModel card)
    {
        if (card.Type != CardType.Skill) return Task.CompletedTask;
        card.EnergyCost.AddThisCombat(-1);
        return Task.CompletedTask;
    }

    public override (PileType, CardPilePosition) ModifyCardPlayResultPileTypeAndPosition(CardModel card, bool isAutoPlay, ResourceInfo resources, PileType pileType, CardPilePosition position)
    {
        if (card.Owner.Creature != Owner || card.Type != CardType.Skill)
        {
            return (pileType, position);
        }
        return (PileType.Exhaust, position);
    }
    */
}
