using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class DaggerSpray2() : ThePrismatic2Card(1, 
    CardType.Attack, CardRarity.Common, 
    TargetType.AllEnemies)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<SilentCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/dagger_spray.png-e68d84c4cddcb97b1e085c7adf6ae00a.ctex";
    public override string PortraitPath => "res://.godot/imported/dagger_spray.png-e68d84c4cddcb97b1e085c7adf6ae00a.ctex";

    public override IEnumerable<CardKeyword> CanonicalKeywords => new _003C_003Ez__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Ethereal);
    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new DamageVar(5m, ValueProp.Move));

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        SfxCmd.Play("event:/sfx/characters/silent/silent_dagger_spray");
        if (CombatState != null)
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).WithHitCount(2).FromCard(this)
                .TargetingAllOpponents(CombatState)
                .WithAttackerFx(() => NDaggerSprayFlurryVfx.Create(Owner.Creature, new Color("#b1ccca"), goingRight: true))
                .BeforeDamage(delegate
                {
                    IReadOnlyList<Creature> hittableEnemies = CombatState.HittableEnemies;
                    foreach (Creature item in hittableEnemies)
                    {
                        NDaggerSprayImpactVfx? child = NDaggerSprayImpactVfx.Create(item, new Color("#b1ccca"), goingRight: true);
                        NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(child);
                    }
                    return Task.CompletedTask;
                })
                .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
    }
}