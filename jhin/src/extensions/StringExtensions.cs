namespace jhin.Extensions;

public static class StringExtensions
{
    private static string Res(params string[] parts)
        => "res://" + string.Join("/", parts);

    // "foo.png".ImagePath()         => "res://JhinMod/Images/foo.png"
    public static string ImagePath(this string path)
        => Res(MainFile.ModId, "Images", path);

    // "foo.png".CardImagePath()     => "res://JhinMod/Images/Cards/foo.png"
    public static string CardImagePath(this string path)
        => Res(MainFile.ModId, "Images", "Cards", path);

    // "foo.png".BigCardImagePath()  => "res://JhinMod/Images/Cards/Big/foo.png"
    public static string BigCardImagePath(this string path)
        => Res(MainFile.ModId, "Images", "Cards", "Big", path);

    // "foo.png".PowerImagePath()    => "res://JhinMod/Images/Powers/foo.png"
    public static string PowerImagePath(this string path)
        => Res(MainFile.ModId, "Images", "Powers", path);

    // "foo.png".BigPowerImagePath() => "res://JhinMod/Images/Powers/Big/foo.png"
    public static string BigPowerImagePath(this string path)
        => Res(MainFile.ModId, "Images", "Powers", "Big", path);

    // "foo.png".RelicImagePath()    => "res://JhinMod/Images/Relics/foo.png"
    public static string RelicImagePath(this string path)
        => Res(MainFile.ModId, "Images", "Relics", path);

    // "foo.png".BigRelicImagePath() => "res://JhinMod/Images/Relics/Big/foo.png"
    public static string BigRelicImagePath(this string path)
        => Res(MainFile.ModId, "Images", "Relics", "Big", path);

    // "foo.png".CharacterUiPath()   => "res://JhinMod/Images/Charui/foo.png"
    public static string CharacterUiPath(this string path)
        => Res(MainFile.ModId, "Images", "Charui", path);
}

public static class Placeholders
{
    public static string Card => "res://" + MainFile.ModId + "/Images/card_placeholder.png";
    public static string Role => "res://" + MainFile.ModId + "/Images/role_placeholder.png";
}
