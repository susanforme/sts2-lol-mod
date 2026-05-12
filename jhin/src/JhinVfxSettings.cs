#nullable enable

namespace jhin;

/// <summary>
/// Centralized presentation tuning for Jhin visual/audio effects.
/// Audio volume is applied on top of the game's public settings save values,
/// so both the game sound settings and these mod-local values affect output.
/// </summary>
public static class JhinVfxSettings
{
    public static class Audio
    {
        /// <summary>
        /// Mod-local master gain for all Jhin one-shot SFX.
        /// Raise/lower this if the whole mod is too quiet/loud relative to the game.
        /// </summary>
        public const float MasterVolumeDb = 0.0f;

        /// <summary>
        /// Local gain for flourish SFX before game SFX bus attenuation is applied.
        /// </summary>
        public const float FlourishVolumeDb = -4.0f;
    }
}
