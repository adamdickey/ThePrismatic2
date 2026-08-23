using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class EchoingSlash2() : ThePrismatic2Card(1, 
    CardType.Attack, CardRarity.Uncommon, 
    TargetType.AllEnemies)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<SilentCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/echoing_slash.png-949e53ef77e71f652b2e83d444084490.ctex";
    public override string PortraitPath => "res://.godot/imported/echoing_slash.png-949e53ef77e71f652b2e83d444084490.ctex";
    
    public override int CanonicalStarCost => 2;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new DamageVar(13m, ValueProp.Move));

    public override IEnumerable<CardKeyword> CanonicalKeywords => new _003C_003Ez__ReadOnlySingleElementList<CardKeyword>(Extensions.Keywords.Starbound);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await using AttackContext attackContext = await AttackCommand.CreateContextAsync(CombatState!, choiceContext, this);
        int attackCount = 1;
        while (attackCount > 0)
        {
            attackCount--;
            IEnumerable<DamageResult> enumerable = await CreatureCmd.Damage(choiceContext, CombatState!.HittableEnemies, DynamicVars.Damage, Owner.Creature, this);
            attackContext.AddHit(enumerable);
            attackCount += enumerable.Count(r => r.WasTargetKilled);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4m);
    }
}