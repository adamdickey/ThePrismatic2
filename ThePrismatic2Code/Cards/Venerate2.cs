using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Venerate2() : ThePrismatic2Card(1, 
    CardType.Skill, CardRarity.Common, 
    TargetType.Self)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<RegentCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/venerate.png-60ee54c883d064c2e935a45afd1268f5.ctex";
    public override string PortraitPath => "res://.godot/imported/venerate.png-60ee54c883d064c2e935a45afd1268f5.ctex";

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new StarsVar(2),
        new DynamicVar("Evoke", 2)
    ]);
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Evoke));

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await PlayerCmd.GainStars(DynamicVars.Stars.BaseValue, Owner);
        if (Owner.PlayerCombatState is { OrbQueue.Orbs.Count: > 0 })
        {
            await OrbCmd.EvokeNext(choiceContext, Owner, dequeue: false);
            await Cmd.CustomScaledWait(0.1f, 0.25f);
            await OrbCmd.EvokeNext(choiceContext, Owner);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Stars.UpgradeValueBy(1m);
    }
}