using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class TeslaCoil2() : ThePrismatic2Card(0, 
    CardType.Attack, CardRarity.Uncommon, 
    TargetType.AnyEnemy)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<DefectCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/tesla_coil.png-1a3be4ec7ac2ecaa46db0f4a60a01990.ctex";
    public override string PortraitPath => "res://.godot/imported/tesla_coil.png-1a3be4ec7ac2ecaa46db0f4a60a01990.ctex";

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new DamageVar(4m, ValueProp.Move),
        new RepeatVar(1)
    ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
        if (Owner.PlayerCombatState != null)
        {
            if (Owner.PlayerCombatState.OrbQueue.Orbs.Count >= 1)
            {
                for (int i = 0; i < DynamicVars.Repeat.IntValue; i++)
                {
                    await OrbCmd.Passive(choiceContext, Owner.PlayerCombatState.OrbQueue.Orbs[0], null);
                    await Cmd.Wait(0.25f);
                }
            }
            if (Owner.PlayerCombatState.OrbQueue.Orbs.Count >= 2)
            {
                for (int i = 0; i < DynamicVars.Repeat.IntValue; i++)
                {
                    await OrbCmd.Passive(choiceContext, Owner.PlayerCombatState.OrbQueue.Orbs[1], null);
                    await Cmd.Wait(0.25f);
                }
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1m);
        DynamicVars.Repeat.UpgradeValueBy(1m);
    }
}