using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Armaments2() : ThePrismatic2Card(1,
    CardType.Skill, CardRarity.Common,
    TargetType.Self)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<IroncladCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/armaments.png-48d04dc8b54801754079bd64a065281d.ctex";
    public override string PortraitPath => "res://.godot/imported/armaments.png-48d04dc8b54801754079bd64a065281d.ctex";
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        HoverTipFactory.Static(StaticHoverTip.Channeling),
        HoverTipFactory.FromOrb<FrostOrb>()
    ]);

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new HpLossVar(1).WithTooltip("BLEED_DYNAMIC"));

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (!Osty.CheckMissingWithAnim(Owner) && Owner.Osty != null)
        {
            VfxCmd.PlayOnCreatureCenter(Owner.Osty, "vfx/vfx_bloody_impact");
            await CreatureCmd.Damage(choiceContext, Owner.Osty, DynamicVars.HpLoss.BaseValue, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this);
        }
        else
        {
            VfxCmd.PlayOnCreatureCenter(Owner.Creature, "vfx/vfx_bloody_impact");
            await CreatureCmd.Damage(choiceContext, Owner.Creature, DynamicVars.HpLoss.BaseValue, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this);
        }
        await OrbCmd.Channel<FrostOrb>(choiceContext, Owner);
        if (IsUpgraded)
        {
            foreach (CardModel item in PileType.Hand.GetPile(Owner).Cards.Where(c => c.IsUpgradable))
            {
                CardCmd.Upgrade(item);
            }
            return;
        }
        CardModel? cardModel = await CardSelectCmd.FromHandForUpgrade(choiceContext, Owner, this);
        if (cardModel != null)
        {
            CardCmd.Upgrade(cardModel);
        }
    }
}