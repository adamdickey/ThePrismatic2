using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class SleightOfFlesh2() : ThePrismatic2Card(2, 
    CardType.Power, CardRarity.Uncommon, 
    TargetType.Self)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<NecrobinderCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/sleight_of_flesh.png-15c5d648d3dba8671cfc07c46c7e6b46.ctex";
    public override string PortraitPath => "res://.godot/imported/sleight_of_flesh.png-15c5d648d3dba8671cfc07c46c7e6b46.ctex";
    
    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new PowerVar<SleightOfFleshPower>(7m));

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "PowerUp", Owner.Character.PowerUpAnimDelay);
        await PowerCmd.Apply<SleightOfFleshPower>(choiceContext, Owner.Creature, DynamicVars["SleightOfFleshPower"].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["SleightOfFleshPower"].UpgradeValueBy(3m);
    }
}