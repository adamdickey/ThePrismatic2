using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class BloodWall2() : ThePrismatic2Card(2, 
    CardType.Skill, CardRarity.Common, 
    TargetType.Self)
{
    
    public override string CustomPortraitPath => "res://.godot/imported/blood_wall.png-b433a4f72503ff9c20356636290c64fe.ctex";
    public override string PortraitPath => "res://.godot/imported/blood_wall.png-b433a4f72503ff9c20356636290c64fe.ctex";

    protected override IEnumerable<string> ExtraRunAssetPaths =>
        new global::_003C_003Ez__ReadOnlySingleElementList<string>(SceneHelper.GetScenePath("vfx/vfx_blood_wall"));

    protected override IEnumerable<DynamicVar> CanonicalVars => new global::_003C_003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
    {
        new HpLossVar(2m),
        new BlockVar(16m, ValueProp.Move)
    });

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (!Osty.CheckMissingWithAnim(base.Owner))
        {
            VfxCmd.PlayOnCreature(base.Owner.Osty, "vfx/vfx_blood_wall");
            await CreatureCmd.Damage(choiceContext, base.Owner.Osty, base.DynamicVars.HpLoss.BaseValue, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this);
        }
        else
        {
            VfxCmd.PlayOnCreature(base.Owner.Creature, "vfx/vfx_blood_wall");
            await CreatureCmd.Damage(choiceContext, base.Owner.Creature, base.DynamicVars.HpLoss.BaseValue, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this);
        }
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        SfxCmd.Play("event:/sfx/characters/ironclad/ironclad_bloodwall");
        await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Block.UpgradeValueBy(4m);
    }
}