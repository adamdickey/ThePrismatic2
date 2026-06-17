using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;
using ThePrismatic2.ThePrismatic2Code.Orbs;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class WroughtInWar2() : ThePrismatic2Card(1, 
    CardType.Attack, CardRarity.Common, 
    TargetType.AnyEnemy)
{
    public override string CustomPortraitPath => "res://.godot/imported/wrought_in_war.png-fbaf5558a1b7b26aa86a5c2d5c71549c.ctex";
    public override string PortraitPath => "res://.godot/imported/wrought_in_war.png-fbaf5558a1b7b26aa86a5c2d5c71549c.ctex";
    
    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new DamageVar(5m, ValueProp.Move),
        new ForgeVar(5)
    ]);
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            List<IHoverTip> list = [HoverTipFactory.Static(StaticHoverTip.Channeling), HoverTipFactory.FromOrb<MagmaOrb>()];
            list.AddRange(HoverTipFactory.FromForge());
            return new _003C_003Ez__ReadOnlyList<IHoverTip>(list);
        }
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_blunt")
            .Execute(choiceContext);
        await ForgeCmd.Forge(DynamicVars.Forge.IntValue, Owner, this);
        await OrbCmd.Channel<MagmaOrb>(choiceContext, Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
        DynamicVars.Forge.UpgradeValueBy(2m);
    }
}