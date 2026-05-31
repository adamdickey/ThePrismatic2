using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using ThePrismatic2.ThePrismatic2Code.Character;
using ThePrismatic2.ThePrismatic2Code.Powers;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class BlackHole2() : ThePrismatic2Card(1, 
    CardType.Power, CardRarity.Uncommon, 
    TargetType.Self)
{
    public override string CustomPortraitPath => "res://.godot/imported/black_hole.png-2729b9b83acc302123ce6e1a8a4d4b71.ctex";
    public override string PortraitPath => "res://.godot/imported/black_hole.png-2729b9b83acc302123ce6e1a8a4d4b71.ctex";

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new PowerVar<BlackHolePower>(3m));

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<BlackHole2Power>(Owner.Creature, DynamicVars["BlackHolePower"].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["BlackHolePower"].UpgradeValueBy(1m);
    }
}