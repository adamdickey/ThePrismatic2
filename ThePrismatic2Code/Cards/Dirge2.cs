using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using ThePrismatic2.ThePrismatic2Code.Character;
using ThePrismatic2.ThePrismatic2Code.Orbs;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Dirge2() : ThePrismatic2Card(0, 
    CardType.Skill, CardRarity.Uncommon, 
    TargetType.Self)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<NecrobinderCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/dirge.png-7dfcb356163c2673892515dd1a73042e.ctex";
    public override string PortraitPath => "res://.godot/imported/dirge.png-7dfcb356163c2673892515dd1a73042e.ctex";
    
    protected override bool HasEnergyCostX => true;

    public override IEnumerable<CardKeyword> CanonicalKeywords => new _003C_003Ez__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlyArray<IHoverTip>([
        HoverTipFactory.Static(StaticHoverTip.Channeling),
        HoverTipFactory.FromOrb<BoneOrb>(),
        HoverTipFactory.FromCard<Soul>(IsUpgraded)
    ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        int xValue = ResolveEnergyXValue();
        for (int i = 0; i < xValue; i++)
        {
            await OrbCmd.Channel<BoneOrb>(choiceContext, Owner);
        }
        if (CombatState != null)
        {
            List<Soul> list = Soul.Create(Owner, xValue, CombatState).ToList();
            if (IsUpgraded)
            {
                foreach (Soul item in list)
                {
                    CardCmd.Upgrade(item);
                }
            }
            CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardsToCombat(list, PileType.Draw, Owner, CardPilePosition.Random));
        }
    }
}