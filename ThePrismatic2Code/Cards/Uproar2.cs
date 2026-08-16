using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

public class Uproar2() : ThePrismatic2Card(2, 
    CardType.Attack, CardRarity.Common, 
    TargetType.AnyEnemy)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<DefectCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/uproar.png-a8bc36a119474d14dc3e3bbac995f2a0.ctex";
    public override string PortraitPath => "res://.godot/imported/uproar.png-a8bc36a119474d14dc3e3bbac995f2a0.ctex";
    
    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new DamageVar(6m, ValueProp.Move));

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).WithHitCount(2)
            .Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
        CardModel? cardModel = PileType.Draw.GetPile(Owner).Cards.Where(c => c.Type == CardType.Attack && !c.Keywords.Contains(CardKeyword.Unplayable)).ToList().StableShuffle(Owner.RunState.Rng.Shuffle)
            .FirstOrDefault();
        if (cardModel == null)
        {
            cardModel = PileType.Draw.GetPile(Owner).Cards.Where(c => c.Type == CardType.Attack).ToList().StableShuffle(Owner.RunState.Rng.Shuffle)
                .FirstOrDefault();
        }
        if (cardModel != null)
        {
            await CardCmd.AutoPlay(choiceContext, cardModel, null);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
    }
}