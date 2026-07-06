using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Models.Powers;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Darkness2() : ThePrismatic2Card(1, 
    CardType.Skill, CardRarity.Uncommon, 
    TargetType.Self)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<DefectCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/darkness.png-5338c66c35c7c51260765ed5a8c9594e.ctex";
    public override string PortraitPath => "res://.godot/imported/darkness.png-5338c66c35c7c51260765ed5a8c9594e.ctex";
    
    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new PowerVar<DoomPower>(10m));
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        HoverTipFactory.Static(StaticHoverTip.Channeling),
        HoverTipFactory.FromOrb<DarkOrb>(),
        HoverTipFactory.FromPower<DoomPower>()
    ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await OrbCmd.Channel<DarkOrb>(choiceContext, Owner);
        Creature? weakestEnemy = CombatState?.HittableEnemies.MinBy(c => c.CurrentHp);
        if (weakestEnemy != null)
        {
            await PowerCmd.Apply<DoomPower>(choiceContext, weakestEnemy, DynamicVars.Doom.BaseValue, Owner.Creature, this);
        }
        
    }
    protected override void OnUpgrade()
    {
        DynamicVars.Doom.UpgradeValueBy(5m);
    }
}