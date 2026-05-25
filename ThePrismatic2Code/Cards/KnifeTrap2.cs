using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class KnifeTrap2() : ThePrismatic2Card(2, 
    CardType.Skill, CardRarity.Rare, 
    TargetType.AnyEnemy)
{
    public override string CustomPortraitPath => "res://.godot/imported/knife_trap.png-92e5249ad1bbb9c3b40b7558322dda80.ctex";
    public override string PortraitPath => "res://.godot/imported/knife_trap.png-92e5249ad1bbb9c3b40b7558322dda80.ctex";

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new CalculationBaseVar(0m),
        new CalculationExtraVar(1m),
        new CalculatedVar("CalculatedCards").WithMultiplier((card, _) => PileType.Exhaust.GetPile(card.Owner).Cards.Count(c => c.VisualCardPool.IsColorless))
    ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        IEnumerable<CardModel> enumerable = PileType.Exhaust.GetPile(Owner).Cards.Where(c => c.VisualCardPool.IsColorless).ToList();
        foreach (CardModel item in enumerable)
        {
            if (IsUpgraded)
            {
                CardCmd.Upgrade(item, CardPreviewStyle.None);
            }
            await CardCmd.AutoPlay(choiceContext, item, cardPlay.Target);
        }
    }
}