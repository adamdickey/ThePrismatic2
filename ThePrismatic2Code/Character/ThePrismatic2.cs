using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using ThePrismatic2.ThePrismatic2Code.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using ThePrismatic2.ThePrismatic2Code.Relics;

namespace ThePrismatic2.ThePrismatic2Code.Character;

public class ThePrismatic2 : PlaceholderCharacterModel
{
    public const string CharacterId = "ThePrismatic2";

    public static readonly Color Color = new("ffffff");

    public override Color NameColor => Color;
    public override CharacterGender Gender => CharacterGender.Neutral;
    public override int StartingHp => 70;

    public override IEnumerable<CardModel> StartingDeck =>
    [
        ModelDb.Card<StrikeIronclad>(),
        ModelDb.Card<StrikeSilent>(),
        ModelDb.Card<StrikeRegent>(),
        ModelDb.Card<StrikeNecrobinder>(),
        ModelDb.Card<DefendIronclad>(),
        ModelDb.Card<DefendSilent>(),
        ModelDb.Card<DefendNecrobinder>(),
        ModelDb.Card<DefendDefect>(),
        ModelDb.Card<Zap>(),
        ModelDb.Card<Venerate>(),
        ModelDb.Card<Survivor>(),
        ModelDb.Card<Bodyguard>()
    ];

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