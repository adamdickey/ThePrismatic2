using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using ThePrismatic2.ThePrismatic2Code.Character;
using ThePrismatic2.ThePrismatic2Code.Powers;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Juggernaut2() : ThePrismatic2Card(2, 
    CardType.Power, CardRarity.Rare, 
    TargetType.Self)
{
    public override string CustomPortraitPath => "res://.godot/imported/juggernaut.png-f6a86f0580ca5b611e7b339d95829bf7.ctex";
    public override string PortraitPath => "res://.godot/imported/juggernaut.png-f6a86f0580ca5b611e7b339d95829bf7.ctex";

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new global::_003C_003Ez__ReadOnlyArray<IHoverTip>(
        new IHoverTip[2]
        {
            HoverTipFactory.Static(StaticHoverTip.Block),
            HoverTipFactory.Static(StaticHoverTip.SummonStatic)
        });

    protected override IEnumerable<DynamicVar> CanonicalVars => new global::_003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new PowerVar<Juggernaut2Power>(5m));

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<Juggernaut2Power>(base.Owner.Creature, base.DynamicVars["Juggernaut2Power"].BaseValue, base.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["Juggernaut2Power"].UpgradeValueBy(2m);
    }
}