using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rewards;

namespace ThePrismatic2.ThePrismatic2Code.Extensions;

public class CardTransformReward(Player player) : Reward(player)
{
    private static string RewardIcon => ImageHelper.GetImagePath("ui/reward_screen/reward_icon_card_removal.png");

    protected override RewardType RewardType => RewardType.RemoveCard;

    public override int RewardsSetIndex => 7;

    protected override string IconPath => RewardIcon;

    public static IEnumerable<string> AssetPaths => new _003C_003Ez__ReadOnlySingleElementList<string>(RewardIcon);

    public override bool IsPopulated => true;

    public override LocString Description => new("gameplay_ui", "COMBAT_REWARD_CARD_TRANSFORM");

    public override void Populate()
    {
    }

    protected override async Task<bool> OnSelect()
    {
        Log.Info($"Player {Player.NetId} obtained card transform from reward");
        return await DoUnsyncedCardTransform(Player);
    }

    public override void MarkContentAsSeen()
    {
    }

    private async Task<bool> DoUnsyncedCardTransform(Player player)
    {
        CardSelectorPrefs prefs = new CardSelectorPrefs(new LocString("gameplay_ui", "COMBAT_REWARD_CARD_TRANSFORM.selectionScreenPrompt"), 1)
        {
            Cancelable = true,
            RequireManualConfirmation = true
        };
        CardModel? card = (await CardSelectCmd.FromDeckForTransformation(player, prefs)).FirstOrDefault();
        if (card != null)
        {
            CardPileAddResult transformCard = await CardCmd.TransformToRandom(card, Player.RunState.Rng.Niche);
            CardCmd.PreviewCardPileAdd(transformCard);
            Log.Debug($"Player {player.NetId} transformed {card.Id}.");
            return true;
        }
        return false;
    }
}