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
public class Anticipate2() : ThePrismatic2Card(0, 
    CardType.Skill, CardRarity.Common, 
    TargetType.Self)
{
    public override string CustomPortraitPath => "res://.godot/imported/anticipate.png-42f2136f7095fe73788485df07f25453.ctex";
    public override string PortraitPath => "res://.godot/imported/anticipate.png-42f2136f7095fe73788485df07f25453.ctex";

    protected override IEnumerable<DynamicVar> CanonicalVars => new global::_003C_003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
    {
        new PowerVar<DexterityPower>(2m),
        new DynamicVar("Focus", 1m)
    });

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new global::_003C_003Ez__ReadOnlyArray<IHoverTip>(new IHoverTip[2]
    {
        HoverTipFactory.FromPower<DexterityPower>(),
        HoverTipFactory.FromPower<FocusPower>()
    });

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<AnticipatePower>(base.Owner.Creature, base.DynamicVars.Dexterity.BaseValue, base.Owner.Creature, this);
        await PowerCmd.Apply<FocusPower>(base.Owner.Creature, base.DynamicVars["Focus"].BaseValue, base.Owner.Creature, this);
        await PowerCmd.Apply<AnticipateFocusPower>(base.Owner.Creature, base.DynamicVars["Focus"].BaseValue, base.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Dexterity.UpgradeValueBy(1m);
        base.DynamicVars["Focus"].UpgradeValueBy(1m);
    }
}