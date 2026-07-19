using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace ThePrismatic2.ThePrismatic2Code.Powers;


public class Corruption2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/corruption_power.png-11d138ae6f08bbf1f5608c82bb178ff9.s3tc.ctex";

    public override string CustomBigIconPath => "res://.godot/imported/corruption_power.png-11d138ae6f08bbf1f5608c82bb178ff9.s3tc.ctex";

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Single;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Exhaust));

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
}
