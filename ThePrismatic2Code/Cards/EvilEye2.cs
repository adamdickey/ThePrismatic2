using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class EvilEye2() : ThePrismatic2Card(1, 
    CardType.Skill, CardRarity.Uncommon, 
    TargetType.Self)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<IroncladCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/evil_eye.png-dbf3e8a2481e5104c82e08adda519d8f.ctex";
    public override string PortraitPath => "res://.godot/imported/evil_eye.png-dbf3e8a2481e5104c82e08adda519d8f.ctex";

    public override bool GainsBlock => true;

    protected override bool ShouldGlowGoldInternal => WasCardExhaustedThisTurn || WasCardDiscardedThisTurn || WasCardCreatedThisTurn;

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new BlockVar(8m, ValueProp.Move));

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Exhaust));

    private bool WasCardExhaustedThisTurn => CombatManager.Instance.History.Entries.OfType<CardExhaustedEntry>().Any(e => e.HappenedThisTurn(CombatState) && e.Card.Owner == Owner);
    private bool WasCardDiscardedThisTurn => CombatManager.Instance.History.Entries.OfType<CardDiscardedEntry>().Any(e => e.HappenedThisTurn(CombatState) && e.Card.Owner == Owner);
    private bool WasCardCreatedThisTurn => CombatManager.Instance.History.Entries.OfType<CardGeneratedEntry>().Any(e => e.HappenedThisTurn(CombatState) && e.Card.Owner == Owner);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        VfxCmd.PlayOnCreatureCenter(Owner.Creature, "vfx/vfx_gaze");
        int blockGains;
        if (WasCardExhaustedThisTurn || WasCardDiscardedThisTurn || WasCardCreatedThisTurn)
        {
            blockGains = 2;
        }
        else
        {
            blockGains = 1;
        }
        for (int i = 0; i < blockGains; i++)
        {
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3m);
    }
}