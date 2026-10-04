using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace ThePrismatic2.ThePrismatic2Code.Relics;

public sealed class NoblesAnvil: ThePrismatic2Relic
{
    public override RelicRarity Rarity => RelicRarity.Rare;
    public override string PackedIconPath => "ThePrismatic2/images/relics/NoblesAnvil.png";
    protected override string PackedIconOutlinePath => "ThePrismatic2/images/relics/NoblesAnvil_Outline.png";
    protected override string BigIconPath => "ThePrismatic2/images/relics/big/NoblesAnvil.png";
    
    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new ForgeVar(5));

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        HoverTipFactory.FromKeyword(Extensions.Keywords.Costly),
        ..HoverTipFactory.FromForge()
    ]);

    private bool _triggeredThisTurn;

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (!_triggeredThisTurn && cardPlay.Card.Owner == Owner && CombatManager.Instance.IsInProgress && cardPlay.Resources.EnergyValue + Math.Max(0, cardPlay.Resources.StarValue) >= 2)
        {
            Flash();
            await ForgeCmd.Forge(DynamicVars.Forge.BaseValue, Owner, this);
            _triggeredThisTurn = true;
        }
    }
    
    public override Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        _triggeredThisTurn = false;
        return Task.CompletedTask;
    }
}
