using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.PotionPools;
using MegaCrit.Sts2.Core.Models.RelicPools;
using jhin.CardPools;
using jhin.Cards;
using jhin.Extensions;
using jhin.Relics;

namespace jhin.Characters;

public class JhinCharacter : PlaceholderCharacterModel
{
    public const string CharacterId = "JHIN";

    public static readonly Color Color = new("b8860b");

    public override string PlaceholderID => "necrobinder";

    public override Color NameColor => Color;
    public override CharacterGender Gender => CharacterGender.Masculine;
    public override int StartingHp => 70;
    public override int StartingGold => 99;

    public override IEnumerable<CardModel> StartingDeck =>
    [
        ModelDb.Card<CommonShot>(),
        ModelDb.Card<CommonShot>(),
        ModelDb.Card<CommonShot>(),
        ModelDb.Card<CommonShot>(),
        ModelDb.Card<GracefulFootwork>(),
        ModelDb.Card<GracefulFootwork>(),
        ModelDb.Card<GracefulFootwork>(),
        ModelDb.Card<GracefulFootwork>(),
        ModelDb.Card<DeadlyFlourish>(),
        ModelDb.Card<Reload>(),
    ];

    public override IReadOnlyList<RelicModel> StartingRelics =>
        new List<RelicModel> { ModelDb.Relic<Whisper>() }.AsReadOnly();

    public override CardPoolModel CardPool => ModelDb.CardPool<JhinCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<SharedRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<SharedPotionPool>();

    public override string CustomVisualPath => "res://scenes/creature_visuals/necrobinder.tscn";
    public override string CustomIconTexturePath => Placeholders.Role;
    public override string CustomCharacterSelectIconPath => Placeholders.Role;
    public override string CustomCharacterSelectLockedIconPath => Placeholders.Role;
    public override string CustomMapMarkerPath => Placeholders.Role;
}
