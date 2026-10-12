using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Mirage2() : ThePrismatic2Card(1, 
    CardType.Skill, CardRarity.Uncommon, 
    TargetType.Self)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<SilentCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/mirage.png-f48d21b445c4c87c049b9c72474c5638.ctex";
    public override string PortraitPath => "res://.godot/imported/mirage.png-f48d21b445c4c87c049b9c72474c5638.ctex";

    public override bool GainsBlock => true;

    public override IEnumerable<CardKeyword> CanonicalKeywords => new _003C_003Ez__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new CalculationBaseVar(8m),
        new CalculationExtraVar(8m),
        new CalculatedBlockVar(ValueProp.Move).WithMultiplier(delegate(CardModel card, Creature? _)
        {
            if (card.CombatState?.Enemies == null) return 0;
            List<ModelId> uniqueDebuffs = [];
            foreach (Creature enemy in card.CombatState.Enemies)
            {
                var debuffs = enemy.Powers.Where(power => power is { Type: PowerType.Debuff, Applier.IsPlayer: true }).ToList();
                foreach (var debuff in debuffs.Where(debuff => !uniqueDebuffs.Contains(debuff.Id)))
                {
                    uniqueDebuffs.Add(debuff.Id);
                }
            }
            return uniqueDebuffs.Count;
        })
    ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.CalculatedBlock.Calculate(cardPlay.Target), DynamicVars.CalculatedBlock.Props, cardPlay);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.CalculationBase.UpgradeValueBy(2m);
        DynamicVars.CalculationExtra.UpgradeValueBy(2m);
    }
}