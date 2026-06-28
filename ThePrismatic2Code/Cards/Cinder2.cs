using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Cinder2() : ThePrismatic2Card(2, 
    CardType.Attack, CardRarity.Common, 
    TargetType.AnyEnemy)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<IroncladCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/cinder.png-d24a89aa723f13b62a5fd061b1faf93f.ctex";
    public override string PortraitPath => "res://.godot/imported/cinder.png-d24a89aa723f13b62a5fd061b1faf93f.ctex";

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>(
    [
        new DamageVar(15m, ValueProp.Move),
            new StarsVar(2)
    ]);

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Ethereal));

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
            .WithHitVfxNode(t => NFireBurstVfx.Create(t, 0.75f))
            .Execute(choiceContext);
        CardPile pile = PileType.Hand.GetPile(Owner);
        CardModel? cardModel = Owner.RunState.Rng.CombatCardSelection.NextItem(pile.Cards);
        if (cardModel != null)
        {
            CardCmd.ApplyKeyword(cardModel, CardKeyword.Ethereal);
            CardCmd.Preview(cardModel, 0.5f);
        }
        await PlayerCmd.GainStars(DynamicVars.Stars.BaseValue, Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(5m);
    }
}