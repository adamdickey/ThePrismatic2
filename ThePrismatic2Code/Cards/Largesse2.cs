using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

public class Largesse2() : ThePrismatic2Card(0, 
    CardType.Skill, CardRarity.Uncommon, 
    TargetType.AnyAlly)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<RegentCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/largesse.png-ba1f10b3dbe82eb4feda7abb09925108.ctex";
    public override string PortraitPath => "res://.godot/imported/largesse.png-ba1f10b3dbe82eb4feda7abb09925108.ctex";
    
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        if (cardPlay.Target.Player != null)
        {
            CardModel? cardModel = CardFactory.GetDistinctForCombat(cardPlay.Target.Player, ModelDb.CardPool<ColorlessCardPool>().GetUnlockedCards(cardPlay.Target.Player.UnlockState, cardPlay.Target.Player.RunState.CardMultiplayerConstraint), 1, Owner.RunState.Rng.CombatCardGeneration).FirstOrDefault();
            if (cardModel != null)
            {
                if (IsUpgraded)
                {
                    CardCmd.Upgrade(cardModel);
                }
                await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Hand, Owner);
            }
        }
    }
}