using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using ThePrismatic2.ThePrismatic2Code.Character;
using ThePrismatic2.ThePrismatic2Code.Powers;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Parry2() : ThePrismatic2Card(1, 
    CardType.Power, CardRarity.Uncommon, 
    TargetType.Self)
{
    public override string CustomPortraitPath => "res://.godot/imported/parry.png-b9dd78d3f3526f2ee704689b657645cb.ctex";
    public override string PortraitPath => "res://.godot/imported/parry.png-b9dd78d3f3526f2ee704689b657645cb.ctex";

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        HoverTipFactory.FromKeyword(Extensions.Keywords.Costly),
        HoverTipFactory.Static(StaticHoverTip.Block)
    ]);

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new PowerVar<ParryPower>(8m));

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<Parry2Power>(Owner.Creature, DynamicVars["ParryPower"].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["ParryPower"].UpgradeValueBy(4m);
    }
}