using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class CloakAndDagger2() : ThePrismatic2Card(1, 
    CardType.Skill, CardRarity.Common, 
    TargetType.Self)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<SilentCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/cloak_and_dagger.png-1e191292ffe94f1e62b2719d8ca4bb2b.ctex";
    public override string PortraitPath => "res://.godot/imported/cloak_and_dagger.png-1e191292ffe94f1e62b2719d8ca4bb2b.ctex";
    
    public override bool GainsBlock => true;

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new BlockVar(6m, ValueProp.Move),
        new CardsVar(1),
        new StarsVar(1)
    ]);

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<Shiv>());

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        for (int i = 0; i < DynamicVars.Cards.IntValue; i++)
        {
            if (CombatState != null) await Shiv.CreateInHand(Owner, CombatState);
            await Cmd.Wait(0.25f);
        }
        await PlayerCmd.GainStars(DynamicVars.Stars.BaseValue, Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1m);
    }
}