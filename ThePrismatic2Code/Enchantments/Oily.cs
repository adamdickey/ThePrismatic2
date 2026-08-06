using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using ThePrismatic2.ThePrismatic2Code.Powers;

namespace ThePrismatic2.ThePrismatic2Code.Enchantments;

public sealed class Oily : EnchantmentModel
{
    public override bool HasExtraCardText => true;

    public override bool ShowAmount => false;

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new PowerVar<WeakPower>(1m),
        new DynamicVar("Exposed", 1m)
    ]);

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        HoverTipFactory.FromPower<WeakPower>(),
        HoverTipFactory.FromPower<ExposedPower>()
        ]);

    public override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay? cardPlay)
    {
        IReadOnlyList<Creature> targets;
        if (Card.TargetType != TargetType.AllEnemies)
        {
            if (cardPlay is { Target: not null })
            {
                IReadOnlyList<Creature> readOnlyList = new _003C_003Ez__ReadOnlySingleElementList<Creature>(cardPlay.Target);
                targets = readOnlyList;
            }
            else
            {
                targets = [];
            }
        }
        else
        {
             targets = Card.CombatState?.HittableEnemies ?? [];
        }
        await PowerCmd.Apply<WeakPower>(choiceContext, targets, DynamicVars.Weak.BaseValue, Card.Owner.Creature, Card);
        await PowerCmd.Apply<ExposedPower>(choiceContext, targets, DynamicVars["Exposed"].BaseValue, Card.Owner.Creature, Card);
    }
}