#nullable enable

using HarmonyLib;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using jhin.Relics;
using jhin.Utils;

namespace jhin.Patches;

[HarmonyPatch]
public static class RelicDescriptionVariablesPatch
{
    private static System.Reflection.MethodBase TargetMethod()
    {
        return AccessTools.PropertyGetter("MegaCrit.Sts2.Core.Models.RelicModel:Description");
    }

    public static void Postfix(RelicModel __instance, ref LocString __result)
    {
        if (__instance is Whisper)
        {
            __result.Add("flourishMultiplierPercent", ConstantUtil.WhisperFlourishBonusPercent);
            __result.Add("lowHpThresholdPercent", ConstantUtil.LowHpThresholdPercent);
            __result.Add("lowHpBonusDamage", ConstantUtil.WhisperLowHpBonusDamage);
            return;
        }

        if (__instance is LastWhisper)
        {
            __result.Add("flourishMultiplierPercent", ConstantUtil.LastWhisperFlourishBonusPercent);
            __result.Add("lowHpThresholdPercent", ConstantUtil.LowHpThresholdPercent);
            __result.Add("lowHpBonusDamage", ConstantUtil.LastWhisperLowHpBonusDamage);
            return;
        }

        if (__instance is JhinMask)
        {
            __result.Add("markAmount", 1);
            return;
        }

        if (__instance is EmptyShell)
        {
            __result.Add("blockAmount", 5);
        }
    }
}
