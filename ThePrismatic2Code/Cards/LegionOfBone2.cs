using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Characters;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

public class LegionOfBone2() : ThePrismatic2Card(2, 
    CardType.Skill, CardRarity.Uncommon, 
    TargetType.AllAllies)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<NecrobinderCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/legion_of_bone.png-1d035e8132a160dfc6298f5854e150d5.ctex";
    public override string PortraitPath => "res://.godot/imported/legion_of_bone.png-1d035e8132a160dfc6298f5854e150d5.ctex";
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => new _003C_003Ez__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new SummonVar(6m));

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.SummonDynamic, DynamicVars.Summon));

    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, Necrobinder.GetSummonAnimIfApplicable(Owner.Character), Necrobinder.GetSummonDelayIfApplicable(Owner.Character));
        IEnumerable<Creature>? enumerable = CombatState?.PlayerCreatures.Where(c => c.IsAlive).ToList();
        if (enumerable != null)
            foreach (Creature item in enumerable)
            {
                if (item.Player != null)
                    await OstyCmd.Summon(choiceContext, item.Player, DynamicVars.Summon.BaseValue, this);
            }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Summon.UpgradeValueBy(2m);
    }
}