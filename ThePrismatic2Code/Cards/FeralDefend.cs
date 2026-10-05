using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Extensions;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

public class FeralDefend() : ThePrismatic2Card(1,
    CardType.Skill, CardRarity.None,
    TargetType.Self)
{
    public override string CustomPortraitPath => "PrismaticDefend.png".BigCardImagePath();
    public override string PortraitPath => "PrismaticDefend.png".CardImagePath();
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Defend];
    
    public override bool IsBasicStrikeOrDefend => false;

    public override bool CanBeGeneratedInCombat => false;

    public override bool GainsBlock => true;

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new BlockVar(6m, ValueProp.Move));

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play);
        IEnumerable<CardModel> enumerable = PileType.Discard.GetPile(Owner).Cards.Where(Filter).ToList().TakeRandom(1, Owner.RunState.Rng.CombatCardSelection);
        foreach (CardModel item in enumerable)
        {
            await CardPileCmd.Add(item, PileType.Hand);
        }
        await Cmd.Wait(0.25f);
    }
    
    private bool Filter(CardModel card)
    {
        bool flag = card.EnergyCost.GetWithModifiers(CostModifiers.All) == 0 && !card.EnergyCost.CostsX;
        bool flag2 = flag;
        if (flag2)
        {
            CardType type = card.Type;
            bool flag3 = (uint)(type - 1) <= 2u;
            flag2 = flag3;
        }
        return flag2;
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3m);
    }
}