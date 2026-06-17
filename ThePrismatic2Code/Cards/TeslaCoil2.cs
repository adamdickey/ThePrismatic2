using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class TeslaCoil2() : ThePrismatic2Card(0, 
    CardType.Attack, CardRarity.Uncommon, 
    TargetType.AnyEnemy)
{
    public override string CustomPortraitPath => "res://.godot/imported/tesla_coil.png-1a3be4ec7ac2ecaa46db0f4a60a01990.ctex";
    public override string PortraitPath => "res://.godot/imported/tesla_coil.png-1a3be4ec7ac2ecaa46db0f4a60a01990.ctex";

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new DamageVar(3m, ValueProp.Move),
        new RepeatVar(2)
        ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
        if (Owner.PlayerCombatState != null)
        {
            List<OrbModel> list = Owner.PlayerCombatState.OrbQueue.Orbs.ToList(); 
            if (list.Count <= DynamicVars.Repeat.BaseValue)
            {
                foreach (var item in list)
                {
                    try
                    {
                        await OrbCmd.Passive(choiceContext, item, cardPlay.Target);
                    }
                    catch
                    {
                        await OrbCmd.Passive(choiceContext, item, null);
                    }
                }
            }
            else
            {
                var randomOrbs = list.OrderBy(_ => Random.Shared.Next()).Take(2).ToList();
                foreach (var item in randomOrbs)
                {
                    try
                    {
                        await OrbCmd.Passive(choiceContext, item, cardPlay.Target);
                    }
                    catch
                    {
                        await OrbCmd.Passive(choiceContext, item, null);
                    }
                }
            }
            
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
    }
}