using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Cards;

namespace ThePrismatic2.ThePrismatic2Code.Powers;

public class MachineLearning2Power : ThePrismatic2Power
{
    public override string CustomPackedIconPath => "res://.godot/imported/machine_learning_power.png-ac7fe6b2e646dee81ef96e975afd73e8.s3tc.ctex";
    public override string CustomBigIconPath => "res://.godot/imported/machine_learning_power.png-ac7fe6b2e646dee81ef96e975afd73e8.s3tc.ctex";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<Soul>());

    public override async Task AfterPlayerTurnStartEarly(PlayerChoiceContext choiceContext, Player player)
    {
        if (Owner.Player != null) CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardsToCombat(Soul.Create(Owner.Player, Amount, CombatState), PileType.Draw, Owner.Player, CardPilePosition.Random));
    }
}
