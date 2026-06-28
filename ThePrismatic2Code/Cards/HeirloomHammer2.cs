using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;


[Pool(typeof(ThePrismatic2CardPool))]
public class HeirloomHammer2() : ThePrismatic2Card(2, 
    CardType.Attack, CardRarity.Rare, 
    TargetType.AnyEnemy)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<RegentCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/heirloom_hammer.png-ef4f5fb87be15d64ded62b3b29885116.ctex";
    public override string PortraitPath => "res://.godot/imported/heirloom_hammer.png-ef4f5fb87be15d64ded62b3b29885116.ctex";

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new DamageVar(20m, ValueProp.Move),
        new RepeatVar(1)
    ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
            .Execute(choiceContext);
        CardModel? selection = (await CardSelectCmd.FromHand(prefs: new CardSelectorPrefs(SelectionScreenPrompt, 1), context: choiceContext, player: Owner, filter: null, source: this)).FirstOrDefault();
        if (selection != null)
        {
            for (int i = 0; i < DynamicVars.Repeat.IntValue; i++)
            {
                CardModel card = selection.CreateClone();
                CardCmd.Upgrade(card);
                await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner);
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(5m);
    }
}