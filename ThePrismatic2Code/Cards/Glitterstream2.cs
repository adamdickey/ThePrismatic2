using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Glitterstream2() : ThePrismatic2Card(2, 
    CardType.Skill, CardRarity.Common, 
    TargetType.Self)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<RegentCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/glitterstream.png-5a5e6cbc4af9d766342dfc7e0aeacf60.ctex";
    public override string PortraitPath => "res://.godot/imported/glitterstream.png-5a5e6cbc4af9d766342dfc7e0aeacf60.ctex";

    public override bool GainsBlock => true;

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new BlockVar(8m, ValueProp.Move),
        new SummonVar(2m),
        new BlockVar("BlockNextTurn", 2m, ValueProp.Move),
        new SummonVar("SummonNextTurn", 2m)
    ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        BlockVar blockVar = (BlockVar)DynamicVars["BlockNextTurn"];
        SummonVar summonVar = (SummonVar)DynamicVars["SummonNextTurn"];
        if (CombatState != null)
        {
            decimal blockNextTurnAmount = Hook.ModifyBlock(CombatState, Owner.Creature, blockVar.BaseValue, blockVar.Props, this, cardPlay, out _);
            decimal summonNextTurnAmount = Hook.ModifySummonAmount(CombatState, Owner, summonVar.BaseValue, this);
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
            await OstyCmd.Summon(choiceContext, Owner, DynamicVars.Summon.BaseValue, this);
            await PowerCmd.Apply<BlockNextTurnPower>(choiceContext, Owner.Creature, blockNextTurnAmount, Owner.Creature, this);
            await PowerCmd.Apply<SummonNextTurnPower>(choiceContext, Owner.Creature, summonNextTurnAmount, Owner.Creature, this);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(2m);
        DynamicVars["BlockNextTurn"].UpgradeValueBy(1m);
    }
}