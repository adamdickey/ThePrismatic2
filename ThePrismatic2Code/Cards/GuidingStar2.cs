using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class GuidingStar2() : ThePrismatic2Card(1, 
    CardType.Attack, CardRarity.Common, 
    TargetType.AnyEnemy)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<RegentCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/guiding_star.png-741b1b575fa11bbd6ddd29ccec79744e.ctex";
    public override string PortraitPath => "res://.godot/imported/guiding_star.png-741b1b575fa11bbd6ddd29ccec79744e.ctex";

    public override int CanonicalStarCost => 1;
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => new _003C_003Ez__ReadOnlySingleElementList<CardKeyword>(Extensions.Keywords.Starbound);
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<Soul>(IsUpgraded));

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new DamageVar(12m, ValueProp.Move));

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        NCreature? nCreature = NCombatRoom.Instance?.GetCreatureNode(cardPlay.Target);
        if (nCreature != null)
        {
            SfxCmd.Play("event:/sfx/characters/regent/regent_guiding_star");
            NSmallMagicMissileVfx? nSmallMagicMissileVfx = NSmallMagicMissileVfx.Create(nCreature.GetBottomOfHitbox(), new Color("50b598"));
            if (NCombatRoom.Instance != null)
                NCombatRoom.Instance.CombatVfxContainer.AddChildSafely(nSmallMagicMissileVfx);
            if (nSmallMagicMissileVfx != null) await Cmd.Wait(nSmallMagicMissileVfx.WaitTime);
        }
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).WithNoAttackerAnim().FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
        if (CombatState != null)
        {
            CardModel card = CombatState.CreateCard<Soul>(Owner);
            if (IsUpgraded)
            {
                CardCmd.Upgrade(card);
            }
            CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Draw, Owner, CardPilePosition.Random));
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1m);
    }
}