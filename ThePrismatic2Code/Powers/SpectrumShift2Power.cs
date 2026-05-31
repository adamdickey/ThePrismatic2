using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class SpectrumShift2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/spectrum_shift_power.png-ba12c7e09d62ed798b503d394d56f70a.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/spectrum_shift_power.png-ba12c7e09d62ed798b503d394d56f70a.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player == Owner.Player)
        {
            List<CardModel> cards = CardFactory.GetDistinctForCombat(Owner.Player, ModelDb.CardPool<ColorlessCardPool>().GetUnlockedCards(Owner.Player.UnlockState, Owner.Player.RunState.CardMultiplayerConstraint), Amount, Owner.Player.RunState.Rng.CombatCardGeneration).ToList();
            List<CardModel> selection = (await CardSelectCmd.FromHand(prefs: new CardSelectorPrefs(CardSelectorPrefs.TransformSelectionPrompt, Amount), context: choiceContext, player: Owner.Player, filter: null, source: this)).ToList() ?? throw new InvalidOperationException();
            foreach (var (i, card) in cards.Index())
            {
                await CardCmd.Transform(selection[i], card);
            }
            Flash();
        }
    }
}
