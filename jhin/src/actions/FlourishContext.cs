namespace jhin.Actions;

/// <summary>
/// Tracks whether the currently resolving card attack is a flourish shot.
/// Set by AbstractShootCard.TryShoot() and consumed by damage modifiers (e.g. Whisper).
/// </summary>
public static class FlourishContext
{
    /// <summary>
    /// True while a flourish attack's damage is being calculated.
    /// </summary>
    public static bool IsActive { get; private set; }

    internal static void Begin()
    {
        IsActive = true;
    }

    internal static void End()
    {
        IsActive = false;
    }
}
