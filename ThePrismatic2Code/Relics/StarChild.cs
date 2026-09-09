using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;

namespace ThePrismatic2.ThePrismatic2Code.Relics;

public sealed class StarChild: ThePrismatic2Relic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    //public override string PackedIconPath => "res://images/atlases/relic_atlas.sprites/burning_blood.tres";
    //protected override string PackedIconOutlinePath => "res://images/atlases/relic_outline_atlas.sprites/burning_blood.tres";
    //protected override string BigIconPath => "res://images/relics/burning_blood.png";

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(Extensions.Keywords.Starbound));

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new StarsVar(2));

    public override async Task AfterRoomEntered(AbstractRoom room)
    {
        if (room is CombatRoom)
        {
            await PlayerCmd.GainStars(DynamicVars.Stars.BaseValue, Owner);
        }
    }
    
    public override Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (participants.Contains(Owner.Creature) && Owner.PlayerCombatState is { TurnNumber: <= 1 })
        {
            IEnumerable<CardModel> enumerable = Owner.PlayerCombatState?.AllCards ?? Array.Empty<CardModel>();
            foreach (CardModel card in enumerable)
            {
                if (card.Type != CardType.Skill || card.Keywords.Contains(Extensions.Keywords.Starbound)) continue;
                card.AddKeyword(Extensions.Keywords.Starbound);
            }
        }
        return Task.CompletedTask;
    }
    
    public override Task AfterCardGeneratedForCombat(CardModel card, Player? creator)
    {
        if (card.Keywords.Contains(Extensions.Keywords.Starbound) || card.Type != CardType.Skill) return Task.CompletedTask;
        card.AddKeyword(Extensions.Keywords.Starbound);
        return Task.CompletedTask;
    }
}