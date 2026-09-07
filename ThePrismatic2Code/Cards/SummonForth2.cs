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
public class SummonForth2() : ThePrismatic2Card(1, 
    CardType.Skill, CardRarity.Uncommon, 
    TargetType.Self)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<RegentCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/summon_forth.png-0c5470285d3094c10ce9d5ac4237292e.ctex";
    public override string PortraitPath => "res://.godot/imported/summon_forth.png-0c5470285d3094c10ce9d5ac4237292e.ctex";

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new ForgeVar(6));

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        ..HoverTipFactory.FromForge(),
        ..HoverTipFactory.FromEnchantment<Oily>()
    ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        if (Owner.PlayerCombatState != null)
        {
            await ForgeCmd.Forge(DynamicVars.Forge.IntValue, Owner, this);
            IEnumerable<SovereignBlade> cards = Owner.PlayerCombatState.AllCards.OfType<SovereignBlade>().Where(delegate(SovereignBlade c)
            {
                CardPile? pile = c.Pile;
                return pile == null || pile.Type != PileType.Hand;
            });
            var sovereignBlades = cards.ToList();
            foreach (SovereignBlade card in Owner.PlayerCombatState.AllCards.OfType<SovereignBlade>())
            {
                CardCmd.Enchant<Oily>(card, 1);
            }
            //IEnumerable<CardModel> costlyCards = from c in Owner.PlayerCombatState.AllCards
                //where c.EnergyCost.GetWithModifiers(CostModifiers.All) + Math.Max(0, c.CurrentStarCost) >= 2 && c.Pile != null && c.Pile.Type != PileType.Hand
                //select c;
            
            //CardModel? costlyCard = Owner.RunState.Rng.CombatCardSelection.NextItem(costlyCards);
            await CardPileCmd.Add(sovereignBlades, PileType.Hand);
            //if (costlyCard != null) await CardPileCmd.Add(costlyCard, PileType.Hand);
        }
        
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Forge.UpgradeValueBy(3m);
    }
}