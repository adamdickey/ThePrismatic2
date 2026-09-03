using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Extensions;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

public class CostlyBlast() : ThePrismatic2Card(2,
    CardType.Attack, CardRarity.Ancient,
    TargetType.AnyEnemy)
{
    public override string CustomPortraitPath => $"PrismaticBlast.png".BigCardImagePath();
    public override string PortraitPath => $"PrismaticBlast.png".CardImagePath();

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new DamageVar(30m, ValueProp.Move));
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(Extensions.Keywords.Costly));

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target)
            .WithHitFx("vfx/vfx_attack_slash", null, "slash_attack.mp3")
            .Execute(choiceContext);

        // Work out which cards count as Costly before touching any of them. Lowering a cost can
        // push a card back under the threshold, so deciding as we go would make a card's fate
        // depend on where it happened to sit in the hand.
        List<CardModel> costlyInHand = PileType.Hand.GetPile(Owner).Cards
            .Where(card => card != this &&
                           card.EnergyCost.GetWithModifiers(CostModifiers.All)
                           + Math.Max(0, card.CurrentStarCost) >= 2)
            .ToList();

        foreach (CardModel card in costlyInHand)
        {
            card.EnergyCost.AddUntilPlayed(-1);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(10m);
    }
}
