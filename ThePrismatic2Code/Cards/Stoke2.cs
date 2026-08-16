using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Stoke2() : ThePrismatic2Card(1, 
    CardType.Skill, CardRarity.Rare, 
    TargetType.Self)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<IroncladCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/stoke.png-da467046f38a5921913a21781e71559b.ctex";
    public override string PortraitPath => "res://.godot/imported/stoke.png-da467046f38a5921913a21781e71559b.ctex";

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        List<CardModel> list = PileType.Hand.GetPile(Owner).Cards.ToList();
        int exhaustCount = list.Count;
        foreach (CardModel item in list)
        {
            await CardCmd.Exhaust(choiceContext, item);
        }
        List<CardModel> cards = CardFactory.GetForCombat(Owner, Owner.Character.CardPool.GetUnlockedCards(Owner.UnlockState, Owner.RunState.CardMultiplayerConstraint), exhaustCount, Owner.RunState.Rng.CombatCardGeneration).ToList();
        if (IsUpgraded)
        {
            CardCmd.Upgrade(cards, CardPreviewStyle.None);
        }
        await CardPileCmd.AddGeneratedCardsToCombat(cards, PileType.Hand, Owner);
    }
}