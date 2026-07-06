using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Extensions;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

public class CoordinatedDefend() : ThePrismatic2Card(1,
    CardType.Skill, CardRarity.Basic,
    TargetType.Self)
{
    public override string CustomPortraitPath => $"PrismaticDefend.png".BigCardImagePath();
    public override string PortraitPath => $"PrismaticDefend.png".CardImagePath();
    
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Defend];
    
    public override bool GainsBlock => true;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new BlockVar(6m, ValueProp.Move),
        new HealVar(3m)
        ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play);
        if (Owner.Osty != null) await CreatureCmd.Heal(Owner.Osty, DynamicVars.Heal.BaseValue);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3m);
    }
}