using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using ThePrismatic2.ThePrismatic2Code.Character;
using ThePrismatic2.ThePrismatic2Code.Powers;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Monologue2() : ThePrismatic2Card(0, 
    CardType.Skill, CardRarity.Uncommon, 
    TargetType.Self)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<RegentCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/monologue.png-3db958f5fb0e9fe0d5e0d37dbaf67542.ctex";
    public override string PortraitPath => "res://.godot/imported/monologue.png-3db958f5fb0e9fe0d5e0d37dbaf67542.ctex";

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new DynamicVar("Strength", 2m),
        new DynamicVar("Focus", 1m)
        ]);

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        HoverTipFactory.FromPower<StrengthPower>(),
        HoverTipFactory.FromPower<FocusPower>()
        ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        Monologue2Power? monologuePower = await PowerCmd.Apply<Monologue2Power>(choiceContext, Owner.Creature, 1m, Owner.Creature, this);
        if (monologuePower != null)
        {
            monologuePower.DynamicVars.Strength.BaseValue = DynamicVars["Strength"].BaseValue;
            monologuePower.DynamicVars["FocusPower"].BaseValue = DynamicVars["Focus"].BaseValue;
        }
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }
}