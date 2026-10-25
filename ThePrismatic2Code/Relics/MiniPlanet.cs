using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using ThePrismatic2.ThePrismatic2Code.Orbs;

namespace ThePrismatic2.ThePrismatic2Code.Relics;

public sealed class MiniPlanet: ThePrismatic2Relic
{
    public override RelicRarity Rarity => RelicRarity.Rare;
    public override string PackedIconPath => "ThePrismatic2/images/relics/MiniPlanet.png";
    protected override string PackedIconOutlinePath => "ThePrismatic2/images/relics/MiniPlanet_Outline.png";
    protected override string BigIconPath => "ThePrismatic2/images/relics/big/MiniPlanet.png";

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        HoverTipFactory.Static(StaticHoverTip.Channeling),
        HoverTipFactory.FromOrb<IronOrb>(),
        HoverTipFactory.FromOrb<MagmaOrb>()
    ]);
    
    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (participants.Contains(Owner.Creature) && Owner.PlayerCombatState is { TurnNumber: <= 1 })
        {
            await OrbCmd.Channel<IronOrb>(new BlockingPlayerChoiceContext(), Owner);
            await OrbCmd.Channel<MagmaOrb>(new BlockingPlayerChoiceContext(), Owner);
        }
    }
}