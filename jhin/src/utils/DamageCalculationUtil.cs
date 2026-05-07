#nullable enable

namespace jhin.Utils;

/// <summary>
/// Detailed damage breakdown for a single shoot damage calculation.
/// </summary>
public readonly record struct ShootDamageCalculationResult(
    int BaseDamage,
    int MarkBonusDamage,
    int CardSpecificMarkBonusDamage,
    int FlatBonusDamage,
    int DamageBeforeWhisper,
    bool AppliedWhisperFlourishMultiplier,
    int DamageAfterWhisperMultiplier,
    int WhisperLowHpBonusDamage,
    int TotalDamage);

public readonly record struct ShootCardDamageInput(
    int DisplayedBaseDamage,
    int ResolvedBaseDamage,
    int MarkStacks,
    int BaseMarkDamagePerStack,
    int AdditionalDamagePerMark,
    int FlatBonusDamage,
    bool IsLowHp,
    decimal DamageMultiplier,
    int PostMultiplierFlatBonusDamage,
    bool IsFlourish);

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
    public static ShootDamageCalculationResult CalculateShootDamage(ShootCardDamageInput input)
    {
        int markBonusDamage = input.MarkStacks * input.BaseMarkDamagePerStack;
        int cardSpecificMarkBonusDamage = input.MarkStacks * input.AdditionalDamagePerMark;
        int damageBeforeWhisper = input.ResolvedBaseDamage + markBonusDamage + cardSpecificMarkBonusDamage + input.FlatBonusDamage;
        bool appliedWhisperFlourishMultiplier = input.DamageMultiplier != 1m;
        int damageAfterWhisperMultiplier = damageBeforeWhisper;
        int whisperLowHpBonusDamage = input.PostMultiplierFlatBonusDamage;

        if (appliedWhisperFlourishMultiplier)
        {
            damageAfterWhisperMultiplier = decimal.ToInt32(decimal.Floor(damageBeforeWhisper * input.DamageMultiplier));
        }

        int totalDamage = damageAfterWhisperMultiplier + whisperLowHpBonusDamage;

        return new ShootDamageCalculationResult(
            BaseDamage: input.ResolvedBaseDamage,
            MarkBonusDamage: markBonusDamage,
            CardSpecificMarkBonusDamage: cardSpecificMarkBonusDamage,
            FlatBonusDamage: input.FlatBonusDamage,
            DamageBeforeWhisper: damageBeforeWhisper,
            AppliedWhisperFlourishMultiplier: appliedWhisperFlourishMultiplier,
            DamageAfterWhisperMultiplier: damageAfterWhisperMultiplier,
            WhisperLowHpBonusDamage: whisperLowHpBonusDamage,
            TotalDamage: totalDamage);
    }

    public static decimal GetShootDamageMultiplier(bool isFlourish, bool hasWhisper, bool hasLastWhisper, bool hasFourthBullet)
    {
        if (!isFlourish)
        {
            return 1m;
        }

        decimal multiplier = 1m;

        if (hasWhisper)
        {
            multiplier *= ConstantUtil.WhisperFlourishMultiplier;
        }

        if (hasLastWhisper)
        {
            multiplier *= ConstantUtil.LastWhisperFlourishMultiplier;
        }

        if (hasFourthBullet)
        {
            multiplier *= 2m;
        }

        return multiplier;
    }

    public static int GetShootPostMultiplierFlatBonus(bool isFlourish, bool isLowHp, bool hasWhisper, bool hasLastWhisper)
    {
        if (!isFlourish || !isLowHp)
        {
            return 0;
        }

        int bonus = 0;

        if (hasWhisper)
        {
            bonus += ConstantUtil.WhisperLowHpBonusDamage;
        }

        if (hasLastWhisper)
        {
            bonus += ConstantUtil.LastWhisperLowHpBonusDamage;
        }

        return bonus;
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
