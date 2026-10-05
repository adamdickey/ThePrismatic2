using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Extensions;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

public class FeralStrike() : ThePrismatic2Card(1,
    CardType.Attack, CardRarity.None,
    TargetType.AnyEnemy), ITranscendenceCard
{
    public override string CustomPortraitPath => "PrismaticStrike.png".BigCardImagePath();
    public override string PortraitPath => "PrismaticStrike.png".CardImagePath();
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Strike];
    
    public override bool IsBasicStrikeOrDefend => false;
    
    public override bool CanBeGeneratedInCombat => false;
    
    public CardModel GetTranscendenceTransformedCard() => ModelDb.Card<FeralBlast>();

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(
        new DamageVar(7m, ValueProp.Move));


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target)
            .WithHitFx("vfx/vfx_attack_slash", null, "slash_attack.mp3")
            .Execute(choiceContext);
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
        DynamicVars.Damage.UpgradeValueBy(3m);
    }
}