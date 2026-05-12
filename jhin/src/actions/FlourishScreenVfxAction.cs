#nullable enable

using Godot;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace jhin.Actions;

/// <summary>
/// Plays a short full-screen flourish effect when Jhin fires the last bullet.
/// The effect is implemented with public Godot nodes only so it stays decoupled
/// from private game internals.
/// </summary>
public static class FlourishScreenVfxAction
{
    private static FlourishScreenVfxNode? _activeVfx;

    public static void Play(PlayerChoiceContext choiceContext, Player player)
    {
        _ = choiceContext;
        _ = player;

        FlourishScreenVfxNode? activeVfx = _activeVfx;
        if (activeVfx is not null && JhinVfx.IsAlive(activeVfx))
        {
            activeVfx.QueueFree();
        }

        _activeVfx = new FlourishScreenVfxNode
        {
            Layer = JhinVfx.DefaultOverlayLayer,
        };

        if (!JhinVfx.TryAddToSceneRoot(_activeVfx))
        {
            _activeVfx = null;
            return;
        }

        JhinVfx.PlayOneShotAudio(JhinAssets.Audio.Placeholder, JhinVfxSettings.Audio.FlourishVolumeDb);
        MainFile.Logger.Info("Flourish: full-screen VFX triggered.");
    }
}

internal sealed partial class FlourishScreenVfxNode : CanvasLayer
{
    private const double TotalDuration = 0.36d;
    private const double FlashPeakTime = 0.055d;
    private const double DarkHoldTime = 0.09d;

    private static readonly Color DarkColor = new(0.02f, 0.00f, 0.045f, 0.0f);
    private static readonly Color GoldFlashColor = new(1.00f, 0.72f, 0.18f, 0.0f);
    private static readonly Color VioletFlashColor = new(0.50f, 0.10f, 0.95f, 0.0f);
    private static readonly Color CurtainColor = new(0.70f, 0.06f, 0.16f, 0.0f);

    private ColorRect? _darken;
    private ColorRect? _goldFlash;
    private ColorRect? _violetFlash;
    private ColorRect? _topCurtain;
    private ColorRect? _bottomCurtain;
    private double _elapsed;

    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Always;

        _darken = JhinVfx.CreateFullScreenRect(DarkColor);
        _violetFlash = JhinVfx.CreateFullScreenRect(VioletFlashColor);
        _goldFlash = JhinVfx.CreateFullScreenRect(GoldFlashColor);

        AddChild(_darken);
        AddChild(_violetFlash);
        AddChild(_goldFlash);

        Vector2 viewportSize = JhinVfx.GetViewportSize(this);
        float curtainHeight = Mathf.Max(18.0f, viewportSize.Y * 0.055f);
        _topCurtain = CreateCurtainRect(viewportSize, curtainHeight, isTop: true);
        _bottomCurtain = CreateCurtainRect(viewportSize, curtainHeight, isTop: false);

        AddChild(_topCurtain);
        AddChild(_bottomCurtain);
    }

    public override void _Process(double delta)
    {
        _elapsed += delta;

        if (_elapsed >= TotalDuration)
        {
            QueueFree();
            return;
        }

        double normalized = _elapsed / TotalDuration;
        double fadeOut = 1.0d - JhinVfx.SmoothStep(0.35d, 1.0d, normalized);

        JhinVfx.SetAlpha(_darken, 0.38f * (float)(normalized < DarkHoldTime / TotalDuration ? 1.0d : fadeOut));
        JhinVfx.SetAlpha(_goldFlash, GetFlashAlpha(maxAlpha: 0.68f));
        JhinVfx.SetAlpha(_violetFlash, GetFlashAlpha(maxAlpha: 0.25f) * (float)fadeOut);
        JhinVfx.SetAlpha(_topCurtain, 0.46f * (float)fadeOut);
        JhinVfx.SetAlpha(_bottomCurtain, 0.46f * (float)fadeOut);
    }

    private static ColorRect CreateCurtainRect(Vector2 viewportSize, float height, bool isTop)
    {
        return JhinVfx.CreateRect(
            CurtainColor,
            new Vector2(0.0f, isTop ? 0.0f : viewportSize.Y - height),
            new Vector2(viewportSize.X, height));
    }

    private float GetFlashAlpha(float maxAlpha)
    {
        if (_elapsed <= FlashPeakTime)
        {
            return maxAlpha * (float)JhinVfx.SmoothStep(0.0d, FlashPeakTime, _elapsed);
        }

        return maxAlpha * (float)(1.0d - JhinVfx.SmoothStep(FlashPeakTime, TotalDuration, _elapsed));
    }
}
