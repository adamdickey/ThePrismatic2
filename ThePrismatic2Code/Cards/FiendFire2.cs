using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class FiendFire2() : ThePrismatic2Card(2, 
    CardType.Attack, CardRarity.Rare, 
    TargetType.AnyEnemy)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<IroncladCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/fiend_fire.png-18563eb83baf0b8fb80f6b4b349846f1.ctex";
    public override string PortraitPath => "res://.godot/imported/fiend_fire.png-18563eb83baf0b8fb80f6b4b349846f1.ctex";
    
    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new DamageVar(7m, ValueProp.Move));

    public override IEnumerable<CardKeyword> CanonicalKeywords => new _003C_003Ez__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);

    protected override IEnumerable<string> ExtraRunAssetPaths => NGroundFireVfx.AssetPaths;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        List<CardModel> list = PileType.Hand.GetPile(Owner).Cards.ToList();
        int cardCount = list.Count;
        foreach (CardModel item in list)
        {
            await CardCmd.Exhaust(choiceContext, item);
        }
        float scale = 0.8f;
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).WithHitCount(cardCount).FromCard(this)
            .Targeting(cardPlay.Target)
            .BeforeDamage(delegate
            {
                NGroundFireVfx? nGroundFireVfx = NGroundFireVfx.Create(cardPlay.Target);
                if (nGroundFireVfx == null)
                {
                    return Task.CompletedTask;
                }
                SfxCmd.Play("event:/sfx/characters/attack_fire");
                nGroundFireVfx.Scale = Vector2.One * scale;
                NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(nGroundFireVfx);
                scale += 0.1f;
                return Task.CompletedTask;
            })
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
    }
}