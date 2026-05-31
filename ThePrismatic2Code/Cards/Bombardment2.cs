using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class Bombardment2() : ThePrismatic2Card(3, 
    CardType.Attack, CardRarity.Rare, 
    TargetType.AnyEnemy)
{
    public override string CustomPortraitPath => "res://.godot/imported/bombardment.png-2970c9b2c5f1fe60fd88db1a7015e3b7.ctex";
    public override string PortraitPath => "res://.godot/imported/bombardment.png-2970c9b2c5f1fe60fd88db1a7015e3b7.ctex";

    public override IEnumerable<CardKeyword> CanonicalKeywords => new _003C_003Ez__ReadOnlyArray<CardKeyword>([CardKeyword.Exhaust, Extensions.Keywords.Cunning]);

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new DamageVar(16m, ValueProp.Move));

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        NCreature? nCreature = NCombatRoom.Instance?.GetCreatureNode(cardPlay.Target);
        if (nCreature != null)
        {
            NLargeMagicMissileVfx? nLargeMagicMissileVfx = NLargeMagicMissileVfx.Create(nCreature.GetBottomOfHitbox(), new Color("50b598"));
            if (NCombatRoom.Instance != null)
                NCombatRoom.Instance.CombatVfxContainer.AddChildSafely(nLargeMagicMissileVfx);
            if (nLargeMagicMissileVfx != null) await Cmd.Wait(nLargeMagicMissileVfx.WaitTime);
        }
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
            .Execute(choiceContext);
    }

    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, CombatState combatState)
    {
        CardPile? pile = Pile;
        if (pile != null && pile.Type == PileType.Exhaust && player == Owner)
        {
            await CardCmd.AutoPlay(choiceContext, this, null);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(5m);
    }
    
    public override Task BeforeCardAutoPlayed(CardModel card, Creature? target, AutoPlayType type)
    {
        if (card == this && type == AutoPlayType.SlyDiscard && CombatManager.Instance.History.CardPlaysStarted.Last().CardPlay.Card == this)
        {
            return Task.FromCanceled(new CancellationToken(true));
        }
        return Task.CompletedTask;
    }
}