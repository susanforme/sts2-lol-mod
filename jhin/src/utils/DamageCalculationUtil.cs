#nullable enable

namespace jhin.Utils;

/// <summary>
/// Detailed damage breakdown for a single shoot damage calculation.
/// </summary>
public readonly record struct ShootDamageCalculationResult(
    int BaseDamage,
    int MarkBonusDamage,
    int CardSpecificMarkBonusDamage,
    int DamageBeforeWhisper,
    bool AppliedWhisperFlourishMultiplier,
    int DamageAfterWhisperMultiplier,
    int WhisperLowHpBonusDamage,
    int TotalDamage);

public static class DamageCalculationUtil
{
    /// <summary>
    /// Returns the Whisper flourish multiplier for the current damage context.
    /// </summary>
    public static decimal GetWhisperFlourishMultiplier(bool hasWhisper, bool isFlourish)
    {
        return hasWhisper && isFlourish
            ? ConstantUtil.WhisperFlourishMultiplier
            : 1m;
    }

    /// <summary>
    /// Returns the Whisper low-HP flat damage bonus for the current damage context.
    /// </summary>
    public static int GetWhisperLowHpBonusDamage(bool hasWhisper, bool isFlourish, bool isLowHp)
    {
        return hasWhisper && isFlourish && isLowHp
            ? ConstantUtil.WhisperLowHpBonusDamage
            : 0;
    }

    /// <summary>
    /// Calculates final shoot damage from base damage, mark stacks, and Whisper-related modifiers.
    /// </summary>
    public static ShootDamageCalculationResult CalculateShootDamage(
        int baseDamage,
        bool isFlourish,
        int markStacks,
        bool isLowHp,
        bool hasWhisper,
        int extraDamagePerMark = 0)
    {
        int markBonusDamage = markStacks * ConstantUtil.MarkDamagePerStack;
        int cardSpecificMarkBonusDamage = markStacks * extraDamagePerMark;
        int damageBeforeWhisper = baseDamage + markBonusDamage + cardSpecificMarkBonusDamage;
        bool appliedWhisperFlourishMultiplier = hasWhisper && isFlourish;
        int damageAfterWhisperMultiplier = damageBeforeWhisper;
        int whisperLowHpBonusDamage = GetWhisperLowHpBonusDamage(hasWhisper, isFlourish, isLowHp);

        if (appliedWhisperFlourishMultiplier)
        {
            damageAfterWhisperMultiplier = decimal.ToInt32(decimal.Floor(damageBeforeWhisper * GetWhisperFlourishMultiplier(hasWhisper, isFlourish)));
        }

        int totalDamage = damageAfterWhisperMultiplier + whisperLowHpBonusDamage;

        return new ShootDamageCalculationResult(
            BaseDamage: baseDamage,
            MarkBonusDamage: markBonusDamage,
            CardSpecificMarkBonusDamage: cardSpecificMarkBonusDamage,
            DamageBeforeWhisper: damageBeforeWhisper,
            AppliedWhisperFlourishMultiplier: appliedWhisperFlourishMultiplier,
            DamageAfterWhisperMultiplier: damageAfterWhisperMultiplier,
            WhisperLowHpBonusDamage: whisperLowHpBonusDamage,
            TotalDamage: totalDamage);
    }

    /// <summary>
    /// Returns whether the target should be treated as low HP for damage bonus checks.
    /// </summary>
    public static bool IsLowHp(int currentHp, int maxHp)
    {
        if (maxHp <= 0)
        {
            return false;
        }

        decimal hpRatio = (decimal)currentHp / maxHp;
        return hpRatio < ConstantUtil.LowHpThreshold;
    }

    /// <summary>
    /// Returns the Last Whisper flourish multiplier for the current damage context.
    /// </summary>
    public static decimal GetLastWhisperFlourishMultiplier(bool hasLastWhisper, bool isFlourish)
    {
        return hasLastWhisper && isFlourish
            ? ConstantUtil.LastWhisperFlourishMultiplier
            : 1m;
    }

    /// <summary>
    /// Returns the Last Whisper low-HP flat damage bonus for the current damage context.
    /// </summary>
    public static int GetLastWhisperLowHpBonusDamage(bool hasLastWhisper, bool isFlourish, bool isLowHp)
    {
        return hasLastWhisper && isFlourish && isLowHp
            ? ConstantUtil.LastWhisperLowHpBonusDamage
            : 0;
    }
}
