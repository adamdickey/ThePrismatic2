using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Hotfix2() : ThePrismatic2Card(0, 
    CardType.Skill, CardRarity.Common, 
    TargetType.Self)
{
    public override string CustomPortraitPath => "res://.godot/imported/hotfix.png-b0dcd8207aa51c29085e0004b77c07ba.ctex";
    public override string PortraitPath => "res://.godot/imported/hotfix.png-b0dcd8207aa51c29085e0004b77c07ba.ctex";
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            List<IHoverTip> list = new List<IHoverTip>();
            list.Add(HoverTipFactory.FromPower<FocusPower>());
            list.AddRange(HoverTipFactory.FromForge());
            return new _003C_003Ez__ReadOnlyList<IHoverTip>(list);
        }
    }

    public override IEnumerable<CardKeyword> CanonicalKeywords => new _003C_003Ez__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new PowerVar<FocusPower>(2m),
        new ForgeVar(5)
        ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<HotfixPower>(Owner.Creature, DynamicVars["FocusPower"].BaseValue, Owner.Creature, this);
        await ForgeCmd.Forge(DynamicVars.Forge.BaseValue, Owner, this);
    }

    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Exhaust);
    }
}