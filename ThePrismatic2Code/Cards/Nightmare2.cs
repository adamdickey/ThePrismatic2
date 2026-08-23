using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Nodes.Vfx.Cards;
using MegaCrit.Sts2.Core.TestSupport;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Nightmare2() : ThePrismatic2Card(1, 
    CardType.Skill, CardRarity.Rare, 
    TargetType.Self)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<SilentCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/nightmare.png-0e5e4a95f852fd9e7ba3c74f8e3c8fab.ctex";
    public override string PortraitPath => "res://.godot/imported/nightmare.png-0e5e4a95f852fd9e7ba3c74f8e3c8fab.ctex";

    public override int CanonicalStarCost => 3;
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => new _003C_003Ez__ReadOnlyArray<CardKeyword>([
        CardKeyword.Exhaust,
        Extensions.Keywords.Starbound
        ]);

    protected override IEnumerable<string> ExtraRunAssetPaths => NNightmareHandsVfx.AssetPaths;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        IEnumerable<CardModel> cards = await CardSelectCmd.FromHand(prefs: new CardSelectorPrefs(SelectionScreenPrompt, 1), context: choiceContext, player: Owner, filter: null, source: this);
        if (TestMode.IsOff && LocalContext.IsMe(Owner))
        {
            NSmokyVignetteVfx? child = NSmokyVignetteVfx.Create(new Color(0.8f, 0.3f, 0.8f, 0.66f), new Color(0f, 0f, 4f, 0.33f));
            NGame.Instance?.CurrentRunNode?.GlobalUi.AddChildSafely(child);
            NGame.Instance?.CurrentRunNode?.GlobalUi.AddChildSafely(NNightmareHandsVfx.Create());
            await Cmd.CustomScaledWait(0.1f, 0.25f);
        }
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        CardModel? selectedCard = cards.FirstOrDefault();
        if (selectedCard != null)
        {
            (await PowerCmd.Apply<NightmarePower>(choiceContext, Owner.Creature, 3m, Owner.Creature, this))?.SetSelectedCard(selectedCard);
        }
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}