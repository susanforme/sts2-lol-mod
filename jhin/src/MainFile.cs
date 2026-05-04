using BaseLib.Utils;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;

namespace jhin;

[ModInitializer(nameof(Initialize))]
public class MainFile
{
    public const string ModId = "JhinMod";
    public const string LocalizationRoot = "res://JhinMod/localization";

    public static Logger Logger { get; } =
        new(ModId, LogType.Generic);

    public static void Initialize()
    {
        Harmony harmony = new(ModId);
        harmony.PatchAll();
    }
}
