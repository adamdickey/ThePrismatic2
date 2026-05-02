using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using ThePrismatic2.ThePrismatic2Code.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Random;
using ThePrismatic2.ThePrismatic2Code.Cards;
using ThePrismatic2.ThePrismatic2Code.Relics;

namespace ThePrismatic2.ThePrismatic2Code.Character;

public class ThePrismatic2 : PlaceholderCharacterModel
{

    public const string CharacterId = "ThePrismatic2";

    public static readonly Color Color = new("ffffff");

    public override Color NameColor => Color;
    public override CharacterGender Gender => CharacterGender.Neutral;
    public override int StartingHp => 70;
    
    public override int BaseOrbSlotCount => 3;

    public override IEnumerable<CardModel> StartingDeck =>
    [
        ..GetRandomStartingDeck()
    ];

    private static IEnumerable<CardModel> GetRandomStartingDeck()
    {
        List<CardModel> strikes1 = [ModelDb.Card<ExposingStrike>(), ModelDb.Card<DoomingStrike>(), ModelDb.Card<ToxicStrike>()];
        List<CardModel> strikes2 = [ModelDb.Card<FleetingStrike>(), ModelDb.Card<BladedStrike>(), ModelDb.Card<GhostlyStrike>()];
        List<CardModel> strikes3 = [ModelDb.Card<LootingStrike>(), ModelDb.Card<ConcentratedStrike>(), ModelDb.Card<CunningStrike>()];
        List<CardModel> strikes4 = [ModelDb.Card<ShockingStrike>(), ModelDb.Card<CosmicStrike>(), ModelDb.Card<LoopingStrike>()];
        List<CardModel> strikes5 = [ModelDb.Card<RecklessStrike>(), ModelDb.Card<NecroStrike>(), ModelDb.Card<NecroStrike>()];
        List<CardModel> strikes6 = [ModelDb.Card<StarryStrike>(), ModelDb.Card<CosmicStrike>(), ModelDb.Card<StarryStrike>()];
        List<CardModel> defends1 = [ModelDb.Card<ExposingDefend>(), ModelDb.Card<DoomingDefend>(), ModelDb.Card<ToxicDefend>()];
        List<CardModel> defends2 = [ModelDb.Card<FleetingDefend>(), ModelDb.Card<BladedDefend>(), ModelDb.Card<GhostlyDefend>()];
        List<CardModel> defends3 = [ModelDb.Card<LootingDefend>(), ModelDb.Card<ConcentratedDefend>(), ModelDb.Card<CunningDefend>()];
        List<CardModel> defends4 = [ModelDb.Card<ShockingDefend>(), ModelDb.Card<CosmicDefend>(), ModelDb.Card<LoopingDefend>()];
        List<CardModel> defends5 = [ModelDb.Card<RecklessDefend>(), ModelDb.Card<NecroDefend>(), ModelDb.Card<NecroDefend>()];
        List<CardModel> defends6 = [ModelDb.Card<StarryDefend>(), ModelDb.Card<CosmicDefend>(), ModelDb.Card<StarryDefend>()];
        List<List<CardModel>> strikes = [strikes1, strikes2, strikes3, strikes4, strikes5, strikes6];
        List<List<CardModel>> defends = [defends1, defends2, defends3, defends4, defends5, defends6];
        while (true)
        {
            List<int> numList = [0, 1, 2, 3, 4, 5];
            numList.StableShuffle(Rng.Chaotic);
            int strikeNum1 = numList[0];
            int defendNum1 = numList[1];
            int strikeNum2 = Rng.Chaotic.NextInt(0, 3);
            int defendNum2 = Rng.Chaotic.NextInt(0, 3);
            CardModel chosenStrike = strikes[strikeNum1][strikeNum2];
            CardModel chosenDefend = defends[defendNum1][defendNum2];
            if (chosenStrike is CosmicStrike & chosenDefend is CosmicDefend)
            {
                continue;
            }
            IEnumerable<CardModel> startingDeck = 
            [
                ModelDb.Card<StrikeIronclad>(), 
                ModelDb.Card<StrikeSilent>(), 
                ModelDb.Card<StrikeRegent>(), 
                ModelDb.Card<StrikeNecrobinder>(), 
                ModelDb.Card<DefendIronclad>(), 
                ModelDb.Card<DefendSilent>(), 
                ModelDb.Card<DefendNecrobinder>(), 
                ModelDb.Card<DefendDefect>(), 
                chosenStrike, 
                chosenDefend
            ];
            return startingDeck;
        }
    }

    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<BurningRing>()
    ];

    public override CardPoolModel CardPool => ModelDb.CardPool<ThePrismatic2CardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<ThePrismatic2RelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<ThePrismatic2PotionPool>();

    /*  PlaceholderCharacterModel will utilize placeholder basegame assets for most of your character assets until you
        override all the other methods that define those assets.
        These are just some of the simplest assets, given some placeholders to differentiate your character with.
        You don't have to, but you're suggested to rename these images. */
    public override Control CustomIcon
    {
        get
        {
            var icon = NodeFactory<Control>.CreateFromResource(CustomIconTexturePath);
            icon.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            return icon;
        }
    }

    public override string CustomIconTexturePath => "character_icon_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectIconPath => "char_select_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_char_name_locked.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "map_marker_char_name.png".CharacterUiPath();
}