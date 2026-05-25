using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class Unmovable2Power : ThePrismatic2Power
{
    
    public override string CustomPackedIconPath => "res://.godot/imported/unmovable_power.png-7fa7fb0a68573669175934d50fc84723.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/unmovable_power.png-7fa7fb0a68573669175934d50fc84723.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>(
        new IHoverTip[2]
        {
            HoverTipFactory.Static(StaticHoverTip.SummonStatic),
            HoverTipFactory.Static(StaticHoverTip.Block)
        });
    
    public override decimal ModifySummonAmount(Player summoner, decimal amount, AbstractModel? source)
    {
        if (source != null && summoner != Owner.Player)
        {
            return amount;
        }
        int num = CombatManager.Instance.History.Entries.OfType<BlockGainedEntry>().Count((BlockGainedEntry e) => e.HappenedThisTurn(CombatState) && e.Actor.Player == summoner && e.Props.IsCardOrMonsterMove() && e.CardPlay.Card != source)
            + CombatManager.Instance.History.Entries.OfType<SummonedEntry>().Count((SummonedEntry e) => e.HappenedThisTurn(CombatState)  && e.Actor.Player == summoner);
        if (num >= Amount)
        {
            return amount;
        }
        return 2m*amount;
    }

    public override decimal ModifyBlockMultiplicative(Creature target, decimal block, ValueProp props, CardModel? cardSource, CardPlay? cardPlay)
    {
        if (target.IsMonster)
        {
            return 1m;
        }
        if (!props.IsCardOrMonsterMove())
        {
            return 1m;
        }
        if (cardSource != null && cardSource.Owner.Creature != Owner)
        {
            return 1m;
        }
        int num = CombatManager.Instance.History.Entries.OfType<BlockGainedEntry>().Count((BlockGainedEntry e) => e.HappenedThisTurn(CombatState) && e.Actor == target && e.Props.IsCardOrMonsterMove() && e.CardPlay != cardPlay)
                  + CombatManager.Instance.History.Entries.OfType<SummonedEntry>().Count((SummonedEntry e) => e.HappenedThisTurn(CombatState)  && e.Actor == target);
        if (num >= Amount)
        {
            return 1m;
        }
        return 2m;
    }
}
