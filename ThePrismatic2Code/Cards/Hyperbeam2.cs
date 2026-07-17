using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Hyperbeam2() : ThePrismatic2Card(2, 
    CardType.Attack, CardRarity.Rare, 
    TargetType.AllEnemies)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<DefectCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/hyperbeam.png-69e686a51ad6411d420df10a5930e442.ctex";
    public override string PortraitPath => "res://.godot/imported/hyperbeam.png-69e686a51ad6411d420df10a5930e442.ctex";
    
    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new DamageVar(28m, ValueProp.Move),
        new PowerVar<StrengthPower>(1m),
        new PowerVar<FocusPower>(1m)
    ]);

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        HoverTipFactory.FromPower<StrengthPower>(),
        HoverTipFactory.FromPower<FocusPower>()
        ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState != null)
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).TargetingAllOpponents(CombatState)
                .WithAttackerAnim("Cast", 0.5f)
                .BeforeDamage(async delegate
                {
                    List<Creature> enemies = CombatState.Enemies.Where(e => e.IsAlive).ToList();
                    NHyperbeamVfx? nHyperbeamVfx = NHyperbeamVfx.Create(Owner.Creature, enemies.Last());
                    if (nHyperbeamVfx != null)
                    {
                        NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(nHyperbeamVfx);
                        await Cmd.Wait(0.5f);
                    }

                    foreach (Creature item in enemies)
                    {
                        NHyperbeamImpactVfx? nHyperbeamImpactVfx = NHyperbeamImpactVfx.Create(Owner.Creature, item);
                        if (nHyperbeamImpactVfx != null)
                        {
                            NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(nHyperbeamImpactVfx);
                        }
                    }
                })
                .Execute(choiceContext);
        await PowerCmd.Apply<StrengthPower>(choiceContext, Owner.Creature, -DynamicVars["StrengthPower"].BaseValue, Owner.Creature, this);
        await PowerCmd.Apply<FocusPower>(choiceContext, Owner.Creature, -DynamicVars["FocusPower"].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(8m);
    }
}