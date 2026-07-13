using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class DeathsDoor2() : ThePrismatic2Card(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<NecrobinderCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/deaths_door.png-ee6a5c8ae4689d500ee53db84ef3c33f.ctex";

    public override string PortraitPath => "res://.godot/imported/deaths_door.png-ee6a5c8ae4689d500ee53db84ef3c33f.ctex";

    public override bool GainsBlock => true;

    protected override bool ShouldGlowGoldInternal => Were2DebuffsAppliedThisTurn;

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new BlockVar(6m, ValueProp.Move),
        new RepeatVar(2)
    ]);

    private bool Were2DebuffsAppliedThisTurn => (from e in CombatManager.Instance.History.Entries.OfType<PowerReceivedEntry>()
        where e.HappenedThisTurn(CombatState) && e.Amount > 0 && e.Power.Type == PowerType.Debuff && e.Applier == Owner.Creature
        select e).Count() >= 2;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        int blockGains = 1;
        if (Were2DebuffsAppliedThisTurn)
        {
            blockGains += DynamicVars.Repeat.IntValue;
        }
        for (int i = 0; i < blockGains; i++)
        {
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(1m);
    }
}