using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class ForgottenRitual2() : ThePrismatic2Card(1, 
    CardType.Skill, CardRarity.Uncommon, 
    TargetType.Self)
{
    public override string CustomPortraitPath => "res://.godot/imported/forgotten_ritual.png-bd77494d8251cc8692e919d82b6a65d9.ctex";
    public override string PortraitPath => "res://.godot/imported/forgotten_ritual.png-bd77494d8251cc8692e919d82b6a65d9.ctex";

    protected override bool ShouldGlowGoldInternal => WasCardExhaustedThisTurn || WasCardDiscardedThisTurn || WasCardCreatedThisTurn;

    public override IEnumerable<CardKeyword> CanonicalKeywords => new _003C_003Ez__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new EnergyVar(3));

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        EnergyHoverTip,
        HoverTipFactory.FromKeyword(CardKeyword.Exhaust)
    ]);

    protected override IEnumerable<string> ExtraRunAssetPaths => NGroundFireVfx.AssetPaths;

    private bool WasCardExhaustedThisTurn => CombatManager.Instance.History.Entries.OfType<CardExhaustedEntry>().Any(e => e.HappenedThisTurn(CombatState) && e.Card.Owner == Owner);
    private bool WasCardDiscardedThisTurn => CombatManager.Instance.History.Entries.OfType<CardDiscardedEntry>().Any(e => e.HappenedThisTurn(CombatState) && e.Card.Owner == Owner);
    private bool WasCardCreatedThisTurn => CombatManager.Instance.History.Entries.OfType<CardGeneratedEntry>().Any(e => e.HappenedThisTurn(CombatState) && e.Card.Owner == Owner);


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(NGroundFireVfx.Create(Owner.Creature, VfxColor.Purple));
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        if (WasCardExhaustedThisTurn || WasCardDiscardedThisTurn || WasCardCreatedThisTurn)
        {
            await PlayerCmd.GainEnergy(DynamicVars.Energy.IntValue, Owner);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Energy.UpgradeValueBy(1m);
    }
}