using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using ThePrismatic2.ThePrismatic2Code.Character;
using ThePrismatic2.ThePrismatic2Code.Extensions;
using ThePrismatic2.ThePrismatic2Code.Powers;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class WraithForm2() : ThePrismatic2Card(3, 
    CardType.Power, CardRarity.Rare, 
    TargetType.Self)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<SilentCardPool>();
    public override string CustomPortraitPath => $"WraithForm.png".BigCardImagePath();
    public override string PortraitPath => $"WraithForm.png".CardImagePath();
    
    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new PowerVar<IntangiblePower>(2m),
        new PowerVar<WraithFormPower>(6m)
    ]);

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        HoverTipFactory.FromPower<IntangiblePower>(),
        HoverTipFactory.FromPower<DoomPower>()
    ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "PowerUp", Owner.Character.PowerUpAnimDelay);
        await PowerCmd.Apply<IntangiblePower>(choiceContext, Owner.Creature, DynamicVars["IntangiblePower"].BaseValue, Owner.Creature, this);
        await PowerCmd.Apply<WraithForm2Power>(choiceContext, Owner.Creature, DynamicVars["WraithFormPower"].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["WraithFormPower"].UpgradeValueBy(-2m);
    }
}