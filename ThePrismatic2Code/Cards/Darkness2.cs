using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using ThePrismatic2.ThePrismatic2Code.Character;
using ThePrismatic2.ThePrismatic2Code.Orbs;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Darkness2() : ThePrismatic2Card(1, 
    CardType.Skill, CardRarity.Uncommon, 
    TargetType.Self)
{
    public override string CustomPortraitPath => "res://.godot/imported/darkness.png-5338c66c35c7c51260765ed5a8c9594e.ctex";
    public override string PortraitPath => "res://.godot/imported/darkness.png-5338c66c35c7c51260765ed5a8c9594e.ctex";
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        HoverTipFactory.Static(StaticHoverTip.Channeling),
        HoverTipFactory.FromOrb<GloomOrb>(),
        HoverTipFactory.FromPower<DoomPower>()
    ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await OrbCmd.Channel<GloomOrb>(choiceContext, Owner);
        IEnumerable<OrbModel>? enumerable = Owner.PlayerCombatState?.OrbQueue.Orbs.Where(orb => orb is GloomOrb);
        int triggerCount = !IsUpgraded ? 1 : 2;
        if (enumerable != null)
            foreach (OrbModel gloomOrb in enumerable)
            {
                for (int i = 0; i < triggerCount; i++)
                {
                    await OrbCmd.Passive(choiceContext, gloomOrb, null);
                }
            }
    }
}