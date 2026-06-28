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

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class SummonForth2() : ThePrismatic2Card(1, 
    CardType.Skill, CardRarity.Uncommon, 
    TargetType.Self)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<RegentCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/summon_forth.png-0c5470285d3094c10ce9d5ac4237292e.ctex";
    public override string PortraitPath => "res://.godot/imported/summon_forth.png-0c5470285d3094c10ce9d5ac4237292e.ctex";

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new ForgeVar(8));

    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            List<IHoverTip> list = new List<IHoverTip>();
            list.AddRange(HoverTipFactory.FromForge());
            list.Add(HoverTipFactory.FromKeyword(CardKeyword.Retain));
            list.Add(HoverTipFactory.FromKeyword(Extensions.Keywords.Costly));
            return new _003C_003Ez__ReadOnlyList<IHoverTip>(list);
        }
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        if (Owner.PlayerCombatState != null)
        {
            IEnumerable<SovereignBlade> cards = Owner.PlayerCombatState.AllCards.OfType<SovereignBlade>().Where(delegate(SovereignBlade c)
            {
                CardPile? pile = c.Pile;
                return pile == null || pile.Type != PileType.Hand;
            });
            var sovereignBlades = cards.ToList();
            IEnumerable<CardModel> costlyCards = from c in Owner.PlayerCombatState.AllCards
                where c.EnergyCost.Canonical + Math.Max(0, c.CurrentStarCost) >= 2 && c.Pile != null && c.Pile.Type != PileType.Hand
                select c;
            
            CardModel? costlyCard = Owner.RunState.Rng.CombatCardSelection.NextItem(costlyCards);
            await CardPileCmd.Add(sovereignBlades, PileType.Hand);
            if (costlyCard != null) await CardPileCmd.Add(costlyCard, PileType.Hand);
        }
        await ForgeCmd.Forge(DynamicVars.Forge.IntValue, Owner, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Forge.UpgradeValueBy(3m);
    }
}