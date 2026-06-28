using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.Vfx.Cards;
using ThePrismatic2.ThePrismatic2Code.Character;
using ThePrismatic2.ThePrismatic2Code.Powers;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class FanOfKnives2() : ThePrismatic2Card(2, 
    CardType.Power, CardRarity.Rare, 
    TargetType.Self)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<SilentCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/fan_of_knives.png-7ba3915fcabeadc8b9905e6d99ac1e62.ctex";
    public override string PortraitPath => "res://.godot/imported/fan_of_knives.png-7ba3915fcabeadc8b9905e6d99ac1e62.ctex";

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new CardsVar("Shivs", 3));

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<Shiv>());

    protected override IEnumerable<string> ExtraRunAssetPaths => NFanOfKnivesVfx.AssetPaths;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<FanOfKnives2Power>(choiceContext, Owner.Creature, 1m, Owner.Creature, this);
        for (int i = 0; i < DynamicVars["Shivs"].IntValue; i++)
        {
            if (CombatState != null) await Shiv.CreateInHand(Owner, CombatState);
            await Cmd.CustomScaledWait(0.1f, 0.2f);
        }
    }

    public override async Task OnEnqueuePlayVfx(Creature? target)
    {
        Owner.Creature.GetBackVfxContainer()?.AddChildSafely(NFanOfKnivesVfx.Create(Owner.Creature));
        await CreatureCmd.TriggerAnim(Owner.Creature, "PowerUp", Owner.Character.PowerUpAnimDelay);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Shivs"].UpgradeValueBy(1m);
    }
}