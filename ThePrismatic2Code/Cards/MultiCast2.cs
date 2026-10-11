using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class MultiCast2() : ThePrismatic2Card(0, 
    CardType.Skill, CardRarity.Rare, 
    TargetType.Self)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<DefectCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/multi_cast.png-81832e5d52543e07fc0de49d16fecc10.ctex";
    public override string PortraitPath => "res://.godot/imported/multi_cast.png-81832e5d52543e07fc0de49d16fecc10.ctex";
    
    private bool HasStarbound => Keywords.Contains(Extensions.Keywords.Starbound) || Keywords.Contains(Extensions.Keywords.StarboundThisTurn);
    protected override bool HasEnergyCostX => true;
    
    public override bool HasStarCostX => HasStarbound;

    public override OrbEvokeType OrbEvokeType => OrbEvokeType.All;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        int evokeCount = ResolveEnergyXValue();
        if (HasStarbound)
        {
            evokeCount += ResolveStarXValue();
        }
        if (IsUpgraded)
        {
            evokeCount++;
        }
        if (Owner.PlayerCombatState != null)
        {
            int orbCount = Owner.PlayerCombatState.OrbQueue.Orbs.Count;
            for (int i = 0; i < orbCount; i++)
            {
                for (int j = 0; j < evokeCount; j++)
                {
                    await OrbCmd.EvokeNext(choiceContext, Owner, j == evokeCount - 1);
                    await Cmd.Wait(0.25f);
                }
            }
        }
    }
}