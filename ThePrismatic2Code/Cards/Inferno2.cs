using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;
using ThePrismatic2.ThePrismatic2Code.Powers;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Inferno2() : ThePrismatic2Card(1, 
    CardType.Power, CardRarity.Uncommon, 
    TargetType.Self)
{
    public override string CustomPortraitPath => "res://.godot/imported/inferno.png-69f23aaa7cb8231fa2b455955f8896e0.ctex";
    public override string PortraitPath => "res://.godot/imported/inferno.png-69f23aaa7cb8231fa2b455955f8896e0.ctex";

    protected override IEnumerable<DynamicVar> CanonicalVars => new global::_003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new PowerVar<InfernoPower>(6m));

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        (await PowerCmd.Apply<Inferno2Power>(base.Owner.Creature, base.DynamicVars["InfernoPower"].BaseValue, base.Owner.Creature, this))?.IncrementSelfDamage();
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["InfernoPower"].UpgradeValueBy(3m);
    }
}