using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using ThePrismatic2.ThePrismatic2Code.Character;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

[Pool(typeof(ThePrismatic2CardPool))]
public class TheSealedThrone2() : ThePrismatic2Card(1, 
    CardType.Power, CardRarity.Ancient, 
    TargetType.Self)
{
    public override string CustomPortraitPath => "res://.godot/imported/the_sealed_throne.png-9a51d17491cc908657d0fea9b763d690.ctex";
    public override string PortraitPath => "res://.godot/imported/the_sealed_throne.png-9a51d17491cc908657d0fea9b763d690.ctex";
    
    public override int CanonicalStarCost => 3;
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => new _003C_003Ez__ReadOnlySingleElementList<CardKeyword>(Extensions.Keywords.Starbound);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<TheSealedThronePower>(Owner.Creature, 1m, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Innate);
    }
}