using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

public class EndOfDays2() : ThePrismatic2Card(3, 
    CardType.Skill, CardRarity.Rare, 
    TargetType.AllEnemies)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<NecrobinderCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/end_of_days.png-91d99e60d59a58525f8e3dfac001038c.ctex";
    public override string PortraitPath => "res://.godot/imported/end_of_days.png-91d99e60d59a58525f8e3dfac001038c.ctex";

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new PowerVar<DoomPower>(29m));

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<DoomPower>());

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        if (CombatState != null)
        {
            Vector2? sideCenterFloor = VfxCmd.GetSideCenterFloor(CombatSide.Enemy, CombatState);
            if (sideCenterFloor.HasValue)
            {
                NLargeMagicMissileVfx? nLargeMagicMissileVfx = NLargeMagicMissileVfx.Create(sideCenterFloor.Value, new Color("8c2447"));
                if (nLargeMagicMissileVfx != null)
                {
                    NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(nLargeMagicMissileVfx);
                    await Cmd.Wait(nLargeMagicMissileVfx.WaitTime);
                }
            }
        }

        if (CombatState?.HittableEnemies != null)
        {
            foreach (Creature hittableEnemy in CombatState.HittableEnemies)
            {
                await PowerCmd.Apply<DoomPower>(choiceContext, hittableEnemy, DynamicVars.Doom.BaseValue,
                    Owner.Creature, this);
            }

            await DoomPower.DoomKill(DoomPower.GetDoomedCreatures(CombatState.HittableEnemies));
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Doom.UpgradeValueBy(8m);
    }
}