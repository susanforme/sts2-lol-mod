using BaseLib.Abstracts;
using Godot;
using jhin.Extensions;

namespace jhin.CardPools;

public class JhinCardPool : CustomCardPoolModel
{
    public override string Title => Characters.JhinCharacter.CharacterId;

    public override string BigEnergyIconPath => Placeholders.Role;
    public override string TextEnergyIconPath => Placeholders.Role;

    public override float H => 0.12f;
    public override float S => 0.95f;
    public override float V => 0.75f;

    public override Color DeckEntryCardColor => new("b8860b");
    public override Color EnergyOutlineColor => new("8b6914");

    public override bool IsColorless => false;
}
