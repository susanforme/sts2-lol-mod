namespace jhin.Utils;

/// <summary>
/// Shared combat constants used by Jhin's current damage systems.
/// </summary>
public static class ConstantUtil
{
    /// <summary>
    /// Bonus damage granted by each Mark stack when a shoot card consumes it.
    /// </summary>
    public const int MarkDamagePerStack = 3;

    /// <summary>
    /// Damage dealt by each Lotus Trap stack when it triggers.
    /// </summary>
    public const int LotusTrapDamagePerStack = 6;

    /// <summary>
    /// Weak applied by each Lotus Trap stack when it triggers.
    /// </summary>
    public const int LotusTrapWeakPerStack = 1;

    /// <summary>
    /// Damage dealt to each living enemy when Lotus Trap kills its owner.
    /// </summary>
    public const int LotusTrapDeathExplosionDamage = 4;

    /// <summary>
    /// Whisper's flourish damage multiplier.
    /// </summary>
    public const decimal WhisperFlourishMultiplier = 1.5m;

    /// <summary>
    /// Integer display value for Whisper's flourish bonus percent.
    /// </summary>
    public static int WhisperFlourishBonusPercent => decimal.ToInt32((WhisperFlourishMultiplier - 1m) * 100m);

    /// <summary>
    /// Extra flat damage from Whisper when the target is below the low-HP threshold.
    /// </summary>
    public const int WhisperLowHpBonusDamage = 6;

    /// <summary>
    /// Last Whisper's flourish damage multiplier (ancient version).
    /// </summary>
    public const decimal LastWhisperFlourishMultiplier = 1.75m;

    /// <summary>
    /// Integer display value for Last Whisper's flourish bonus percent.
    /// </summary>
    public static int LastWhisperFlourishBonusPercent => decimal.ToInt32((LastWhisperFlourishMultiplier - 1m) * 100m);

    /// <summary>
    /// Extra flat damage from Last Whisper when the target is below the low-HP threshold.
    /// </summary>
    public const int LastWhisperLowHpBonusDamage = 12;

    /// <summary>
    /// HP ratio below which a target counts as low HP.
    /// </summary>
    public const decimal LowHpThreshold = 0.25m;

    /// <summary>
    /// Integer display value for the low-HP threshold percent.
    /// </summary>
    public static int LowHpThresholdPercent => decimal.ToInt32(LowHpThreshold * 100m);

    /// <summary>
    /// Curtain Call gains up to this multiplier as the target approaches 0 HP.
    /// </summary>
    public const decimal CurtainCallMissingHpMultiplierScale = 1m;

    public static int CurtainCallMaxDamageBonusPercent => decimal.ToInt32(CurtainCallMissingHpMultiplierScale * 100m);
}
