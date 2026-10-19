using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using ThePrismatic2.ThePrismatic2Code.Enchantments;

namespace ThePrismatic2.ThePrismatic2Code.Relics;

public sealed class StarChild: ThePrismatic2Relic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    public override string PackedIconPath => "ThePrismatic2/images/relics/StarChild.png";
    protected override string PackedIconOutlinePath => "ThePrismatic2/images/relics/StarChild_Outline.png";
    protected override string BigIconPath => "ThePrismatic2/images/relics/big/StarChild.png";

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        ..HoverTipFactory.FromEnchantment<Starbound>()
    ]);

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new CardsVar(3),
        new StarsVar(1)
    ]);

    public override async Task AfterObtained()
    {
        foreach (CardModel item in await CardSelectCmd.FromDeckForEnchantment(prefs: new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, DynamicVars.Cards.IntValue), player: Owner, enchantment: ModelDb.Enchantment<Starbound>(), amount: 1))
        {
            CardCmd.Enchant<Starbound>(item, 1m);
            NCardEnchantVfx? nCardEnchantVfx = NCardEnchantVfx.Create(item);
            if (nCardEnchantVfx != null)
            {
                NRun.Instance?.GlobalUi.CardPreviewContainer.AddChildSafely(nCardEnchantVfx);
            }
        }
    }
    
    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (participants.Contains(Owner.Creature))
        {
            await PlayerCmd.GainStars(DynamicVars.Stars.BaseValue, Owner);
        }
    }
}
