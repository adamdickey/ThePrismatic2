using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using ThePrismatic2.ThePrismatic2Code.Extensions;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

public class LoopingStrike() : ThePrismatic2Card(1,
    CardType.Attack, CardRarity.None,
    TargetType.AnyEnemy), ITranscendenceCard
{
    public override string CustomPortraitPath => $"PrismaticStrike.png".BigCardImagePath();
    public override string PortraitPath => $"PrismaticStrike.png".CardImagePath();
    protected override HashSet<CardTag> CanonicalTags => [ CardTag.Strike ];
    
    public override bool IsBasicStrikeOrDefend => false;
    
    public override bool CanBeGeneratedInCombat => false;
    
    public CardModel GetTranscendenceTransformedCard() => ModelDb.Card<LoopingBlast>();

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new DamageVar(7m, ValueProp.Move),
        new RepeatVar(2)
        ]);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target)
            .WithHitFx("vfx/vfx_attack_slash", null, "slash_attack.mp3")
            .Execute(choiceContext);
        if (Owner.PlayerCombatState != null && Owner.PlayerCombatState.OrbQueue.Orbs.Count != 0)
        {
            for (int i = 0; i < DynamicVars.Repeat.IntValue; i++)
            {
                await OrbCmd.Passive(choiceContext, Owner.PlayerCombatState.OrbQueue.Orbs[0], null);
                await Cmd.Wait(0.25f);
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
    }
}