using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class CrashLanding2() : ThePrismatic2Card(1, 
    CardType.Attack, CardRarity.Rare, 
    TargetType.AllEnemies)
{
    public override string CustomPortraitPath => "res://.godot/imported/crash_landing.png-9d1701ec29fc8246f7d98effe8fe35da.ctex";
    public override string PortraitPath => "res://.godot/imported/crash_landing.png-9d1701ec29fc8246f7d98effe8fe35da.ctex";

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<Forget>());

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new DamageVar(21m, ValueProp.Move));

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState != null)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).TargetingAllOpponents(CombatState)
                .WithHitFx("vfx/vfx_heavy_blunt", null, "heavy_attack.mp3")
                .WithHitVfxSpawnedAtBase()
                .Execute(choiceContext);
            int num = 10 - CardPile.GetCards(Owner, PileType.Hand).Count();
            List<CardModel> list = new List<CardModel>();
            for (int i = 0; i < num; i++)
            {
                list.Add(CombatState.CreateCard<Forget>(Owner));
            }

            await CardPileCmd.AddGeneratedCardsToCombat(list, PileType.Hand, addedByPlayer: true);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(5m);
    }
}