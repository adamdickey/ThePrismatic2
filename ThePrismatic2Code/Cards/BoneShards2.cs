using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class BoneShards2() : ThePrismatic2Card(1, 
    CardType.Attack, CardRarity.Uncommon, 
    TargetType.AllEnemies)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<NecrobinderCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/bone_shards.png-6f26fa090e9e316c17c061ccaebb2b7f.ctex";
    public override string PortraitPath => "res://.godot/imported/bone_shards.png-6f26fa090e9e316c17c061ccaebb2b7f.ctex";
    
    protected override bool ShouldGlowGoldInternal => !Owner.IsOstyMissing;

    public override bool GainsBlock => true;

    protected override HashSet<CardTag> CanonicalTags => [CardTag.OstyAttack];

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new HpLossVar(4m),
        new OstyDamageVar(9m, ValueProp.Move),
        new DamageVar(9m, ValueProp.Move),
        new BlockVar(9m, ValueProp.Move)
    ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (!Osty.CheckMissingWithAnim(Owner) && Owner.Osty != null && CombatState != null)
        {
            await DamageCmd.Attack(DynamicVars.OstyDamage.BaseValue).FromOsty(Owner.Osty, this)
                .TargetingAllOpponents(CombatState)
                .WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
                .Execute(choiceContext);
        }
        else
        {
            if (CombatState != null)
                await DamageCmd.Attack(DynamicVars.OstyDamage.BaseValue).FromCard(this)
                    .TargetingAllOpponents(CombatState)
                    .WithHitFx("vfx/vfx_attack_blunt", null, "heavy_attack.mp3")
                    .Execute(choiceContext);
        }
        if (!Osty.CheckMissingWithAnim(Owner) && Owner.Osty != null)
        {
            await CreatureCmd.Damage(choiceContext, Owner.Osty, DynamicVars.HpLoss.BaseValue, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this);
        }
        else
        {
            await CreatureCmd.Damage(choiceContext, Owner.Creature, DynamicVars.HpLoss.BaseValue, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this);
        }
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.OstyDamage.UpgradeValueBy(3m);
        DynamicVars.Block.UpgradeValueBy(3m);
    }
}