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
using ThePrismatic2.ThePrismatic2Code.Orbs;
using ThePrismatic2.ThePrismatic2Code.Powers;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class ConsumingShadow2() : ThePrismatic2Card(2, 
    CardType.Power, CardRarity.Rare, 
    TargetType.Self)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<DefectCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/consuming_shadow.png-36fbfe0f9417015a906c77ca4a6f42a4.ctex";
    public override string PortraitPath => "res://.godot/imported/consuming_shadow.png-36fbfe0f9417015a906c77ca4a6f42a4.ctex";
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        HoverTipFactory.Static(StaticHoverTip.Channeling),
        HoverTipFactory.FromOrb<GloomOrb>(),
        HoverTipFactory.FromPower<DoomPower>(),
        HoverTipFactory.Static(StaticHoverTip.Evoke)
    ]);

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new RepeatVar(2),
        new PowerVar<ConsumingShadowPower>(1m)
    ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        for (int i = 0; i < DynamicVars.Repeat.IntValue; i++)
        {
            await OrbCmd.Channel<GloomOrb>(choiceContext, Owner);
        }
        await PowerCmd.Apply<ConsumingShadow2Power>(choiceContext, Owner.Creature, DynamicVars["ConsumingShadowPower"].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Repeat.UpgradeValueBy(1m);
    }
}