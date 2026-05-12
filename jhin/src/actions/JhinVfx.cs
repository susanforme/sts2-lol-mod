#nullable enable

using Godot;
using MegaCrit.Sts2.Core.Saves;

namespace jhin.Actions;

/// <summary>
/// Shared visual/audio helpers for Jhin combat presentation.
/// Keep this class limited to public Godot APIs so individual effects do not
/// depend on private game scene internals.
/// </summary>
public static class JhinVfx
{
    public const int DefaultOverlayLayer = 512;

    private static readonly Dictionary<string, AudioStream> AudioCache = [];

    public static Node? GetSceneRoot()
    {
        return Engine.GetMainLoop() is SceneTree sceneTree ? sceneTree.Root : null;
    }

    public static bool TryAddToSceneRoot(Node node)
    {
        Node? root = GetSceneRoot();
        if (root is null)
        {
            MainFile.Logger.Warn($"Jhin VFX skipped: SceneTree root is unavailable for {node.GetType().Name}.");
            return false;
        }

        root.AddChild(node);
        return true;
    }

    public static bool IsAlive(GodotObject? godotObject)
    {
        return godotObject is not null
            && GodotObject.IsInstanceValid(godotObject)
            && (godotObject is not Node node || !node.IsQueuedForDeletion());
    }

    public static Vector2 GetViewportSize(Node node)
    {
        return node.GetViewport()?.GetVisibleRect().Size ?? new Vector2(1920.0f, 1080.0f);
    }

    public static ColorRect CreateFullScreenRect(Color color)
    {
        ColorRect rect = new()
        {
            Color = color,
            MouseFilter = Control.MouseFilterEnum.Ignore,
        };

        rect.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
        return rect;
    }

    public static ColorRect CreateRect(Color color, Vector2 position, Vector2 size)
    {
        return new ColorRect
        {
            Color = color,
            MouseFilter = Control.MouseFilterEnum.Ignore,
            Position = position,
            Size = size,
        };
    }

    public static void SetAlpha(ColorRect? rect, float alpha)
    {
        if (rect is null)
        {
            return;
        }

        Color color = rect.Color;
        color.A = Mathf.Clamp(alpha, 0.0f, 1.0f);
        rect.Color = color;
    }

    public static double SmoothStep(double edge0, double edge1, double value)
    {
        if (edge0 >= edge1)
        {
            return value < edge0 ? 0.0d : 1.0d;
        }

        double x = Math.Clamp((value - edge0) / (edge1 - edge0), 0.0d, 1.0d);
        return x * x * (3.0d - 2.0d * x);
    }

    public static void PlayOneShotAudio(string resourcePath, float volumeDb = 0.0f)
    {
        Node? root = GetSceneRoot();
        if (root is null)
        {
            MainFile.Logger.Warn($"Jhin audio skipped: SceneTree root is unavailable for {resourcePath}.");
            return;
        }

        AudioStream? stream = LoadAudioStream(resourcePath);
        if (stream is null)
        {
            return;
        }

        AudioStreamPlayer player = new()
        {
            Stream = stream,
            VolumeDb = GetEffectiveSfxVolumeDb(volumeDb),
        };

        player.Finished += player.QueueFree;
        root.AddChild(player);
        player.Play();
    }

    private static AudioStream? LoadAudioStream(string resourcePath)
    {
        if (AudioCache.TryGetValue(resourcePath, out AudioStream? cachedStream))
        {
            return cachedStream;
        }

        AudioStream? stream = ResourceLoader.Load<AudioStream>(resourcePath);
        if (stream is null)
        {
            MainFile.Logger.Warn($"Jhin audio skipped: failed to load {resourcePath}.");
            return null;
        }

        AudioCache[resourcePath] = stream;
        return stream;
    }

    public static float GetEffectiveSfxVolumeDb(float localVolumeDb)
    {
        float effectiveLinearVolume = GetGameSfxLinearVolume() * DbToLinear(JhinVfxSettings.Audio.MasterVolumeDb + localVolumeDb);
        return effectiveLinearVolume <= 0.0f ? -80.0f : Mathf.LinearToDb(effectiveLinearVolume);
    }

    private static float GetGameSfxLinearVolume()
    {
        SettingsSave settings = SaveManager.Instance.SettingsSave;
        return Mathf.Clamp(settings.VolumeMaster * settings.VolumeSfx, 0.0f, 1.0f);
    }

    private static float DbToLinear(float db)
    {
        return Mathf.Pow(10.0f, db / 20.0f);
    }
}
