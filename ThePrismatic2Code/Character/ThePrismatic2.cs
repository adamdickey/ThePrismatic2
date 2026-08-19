using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using ThePrismatic2.ThePrismatic2Code.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Models;
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

    public static IEnumerable<CardModel> GetRandomStartingDeck()
    {
        while (true)
        {
            CardModel chosenStrike = GetRandomPrismaticStrike();
            CardModel chosenDefend = GetRandomPrismaticDefend();
            if (chosenStrike is DoomingStrike or ExposingStrike or ToxicStrike & chosenDefend is DoomingDefend or ExposingDefend or ToxicDefend)
            {
                continue;
            }
            if (chosenStrike is BladedStrike or FleetingStrike or GhostlyStrike & chosenDefend is BladedDefend or FleetingDefend or GhostlyDefend)
            {
                continue;
            }
            if (chosenStrike is ConcentratedStrike or CunningStrike or LootingStrike & chosenDefend is ConcentratedDefend or CunningDefend or LootingDefend)
            {
                continue;
            }
            if (chosenStrike is CosmicStrike or LoopingStrike or ToxicStrike & chosenDefend is CosmicDefend or LoopingDefend or ToxicDefend)
            {
                continue;
            }
            if (chosenStrike is CoordinatedStrike or NecroStrike or RecklessStrike & chosenDefend is CoordinatedDefend or NecroDefend or RecklessDefend)
            {
                continue;
            }
            if (chosenStrike is CosmicStrike or StarboundStrike or StarryStrike & chosenDefend is CosmicDefend or StarboundDefend or StarryDefend)
            {
                continue;
            }
            if (chosenStrike is ClawingStrike or CostlyStrike or ForgingStrike & chosenDefend is ClawingDefend or CostlyDefend or ForgingDefend)
            {
                continue;
            }
            IEnumerable<CardModel> startingDeck = 
            [
                ModelDb.Card<StrikePrismatic>(),
                ModelDb.Card<StrikePrismatic>(),
                ModelDb.Card<StrikePrismatic>(),
                ModelDb.Card<StrikePrismatic>(),
                ModelDb.Card<DefendPrismatic>(),
                ModelDb.Card<DefendPrismatic>(),
                ModelDb.Card<DefendPrismatic>(),
                ModelDb.Card<DefendPrismatic>(),
                chosenStrike, 
                chosenDefend
            ];
            return startingDeck;
        }
    }

    public static CardModel GetRandomPrismaticStrike()
    {
        List<CardModel> strikes = 
        [ModelDb.Card<ExposingStrike>(), ModelDb.Card<DoomingStrike>(), ModelDb.Card<ToxicStrike>(),
            ModelDb.Card<FleetingStrike>(), ModelDb.Card<BladedStrike>(), ModelDb.Card<GhostlyStrike>(),
            ModelDb.Card<LootingStrike>(), ModelDb.Card<ConcentratedStrike>(), ModelDb.Card<CunningStrike>(),
            ModelDb.Card<ToxicStrike>(), ModelDb.Card<CosmicStrike>(), ModelDb.Card<LoopingStrike>(),
            ModelDb.Card<RecklessStrike>(), ModelDb.Card<NecroStrike>(), ModelDb.Card<CoordinatedStrike>(),
            ModelDb.Card<StarryStrike>(), ModelDb.Card<CosmicStrike>(), ModelDb.Card<StarboundStrike>(),
            ModelDb.Card<ClawingStrike>(), ModelDb.Card<CostlyStrike>(), ModelDb.Card<ForgingStrike>()];
        return strikes.TakeRandom(1, Rng.Chaotic).First();
    }
    
    public static CardModel GetRandomPrismaticDefend()
    {
        List<CardModel> defends = 
        [ModelDb.Card<ExposingDefend>(), ModelDb.Card<DoomingDefend>(), ModelDb.Card<ToxicDefend>(),
            ModelDb.Card<FleetingDefend>(), ModelDb.Card<BladedDefend>(), ModelDb.Card<GhostlyDefend>(),
            ModelDb.Card<LootingDefend>(), ModelDb.Card<ConcentratedDefend>(), ModelDb.Card<CunningDefend>(),
            ModelDb.Card<ToxicDefend>(), ModelDb.Card<CosmicDefend>(), ModelDb.Card<LoopingDefend>(),
            ModelDb.Card<RecklessDefend>(), ModelDb.Card<NecroDefend>(), ModelDb.Card<CoordinatedDefend>(),
            ModelDb.Card<StarryDefend>(), ModelDb.Card<CosmicDefend>(), ModelDb.Card<StarboundDefend>(),
            ModelDb.Card<ClawingDefend>(), ModelDb.Card<CostlyDefend>(), ModelDb.Card<ForgingDefend>()];
        return defends.TakeRandom(1, Rng.Chaotic).First();
    }

    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<VividImagination>()
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