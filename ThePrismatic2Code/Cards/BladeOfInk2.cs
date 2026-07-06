using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using ThePrismatic2.ThePrismatic2Code.Character;
using ThePrismatic2.ThePrismatic2Code.Enchantments;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class BladeOfInk2() : ThePrismatic2Card(1, 
    CardType.Skill, CardRarity.Rare, 
    TargetType.Self)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<SilentCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/blade_of_ink.png-fc2ab0e52c6a3c27f3cdec726b4e58c4.ctex";
    public override string PortraitPath => "res://.godot/imported/blade_of_ink.png-fc2ab0e52c6a3c27f3cdec726b4e58c4.ctex";
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            List<IHoverTip> list =
            [
                HoverTipFactory.FromCard<Shiv>()
            ];
            list.AddRange(HoverTipFactory.FromEnchantment<Inked>());
            return new _003C_003Ez__ReadOnlyList<IHoverTip>(list);
        }
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new CardsVar(2));

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState != null)
            foreach (CardModel item in await Shiv.CreateInHand(Owner, DynamicVars.Cards.IntValue, CombatState))
            {
                CardCmd.Enchant<Inked>(item, 1m);
            }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1m);
    }
}