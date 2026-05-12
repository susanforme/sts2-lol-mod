#nullable enable

using Godot;
using MegaCrit.Sts2.Core.Entities.Creatures;

namespace jhin.Actions;

public static class CurtainCallVfxAction
{
    private const int OpeningDelayMs = 120;
    private const int ShotDelayMs = 90;
    private const int FinalShotDelayMs = 170;

    public static async Task PlayOpening()
    {
        CurtainCallOpeningVfxNode openingVfx = new()
        {
            Layer = JhinVfx.DefaultOverlayLayer - 1,
        };

        if (JhinVfx.TryAddToSceneRoot(openingVfx))
        {
            MainFile.Logger.Info("Curtain Call: opening VFX prepared.");
        }

        await Task.Delay(OpeningDelayMs);
    }

    public static async Task PlayShot(int shotNumber, Creature target, bool isFinalShot)
    {
        CurtainCallShotVfxNode shotVfx = new(shotNumber, isFinalShot)
        {
            Layer = JhinVfx.DefaultOverlayLayer,
        };

        if (JhinVfx.TryAddToSceneRoot(shotVfx))
        {
            JhinVfx.PlayOneShotAudio(JhinAssets.Audio.Placeholder, JhinVfxSettings.Audio.CurtainCallShotVolumeDb);
        }

        MainFile.Logger.Info(isFinalShot
            ? $"Curtain Call: heavy fourth shot line locked on {target.LogName}."
            : $"Curtain Call: sniper line {shotNumber} locked on {target.LogName}.");

        await Task.Delay(isFinalShot ? FinalShotDelayMs : ShotDelayMs);
    }
}

internal sealed partial class CurtainCallOpeningVfxNode : CanvasLayer
{
    private const double TotalDuration = 0.42d;
    private const double FadeInEnd = 0.16d;
    private const float DarkMaxAlpha = 0.42f;
    private const float LineMaxAlpha = 0.36f;
    private const float HorizontalLineRatio = 0.72f;
    private const float VerticalLineRatio = 0.46f;

    private static readonly Color DarkColor = new(0.015f, 0.0f, 0.035f, 0.0f);
    private static readonly Color LineColor = new(0.95f, 0.62f, 0.16f, 0.0f);

    private ColorRect? _darken;
    private ColorRect? _horizontalLine;
    private ColorRect? _verticalLine;
    private double _elapsed;

    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Always;

        Vector2 viewportSize = JhinVfx.GetViewportSize(this);
        Vector2 center = viewportSize / 2.0f;

        _darken = JhinVfx.CreateFullScreenRect(DarkColor);
        _horizontalLine = JhinVfx.CreateLine(
            LineColor,
            center,
            new Vector2(viewportSize.X * HorizontalLineRatio, 2.0f));
        _verticalLine = JhinVfx.CreateLine(
            LineColor,
            center,
            new Vector2(2.0f, viewportSize.Y * VerticalLineRatio));

        AddChild(_darken);
        AddChild(_horizontalLine);
        AddChild(_verticalLine);
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
        double fadeIn = JhinVfx.SmoothStep(0.0d, FadeInEnd, _elapsed);
        double fadeOut = 1.0d - JhinVfx.SmoothStep(0.45d, 1.0d, normalized);
        float alphaScale = (float)(fadeIn * fadeOut);

        JhinVfx.SetAlpha(_darken, DarkMaxAlpha * alphaScale);
        JhinVfx.SetAlpha(_horizontalLine, LineMaxAlpha * alphaScale);
        JhinVfx.SetAlpha(_verticalLine, LineMaxAlpha * alphaScale);
    }
}

internal sealed partial class CurtainCallShotVfxNode : CanvasLayer
{
    private const double NormalDuration = 0.24d;
    private const double FinalDuration = 0.36d;
    private const double FlashPeakTime = 0.045d;
    private const float NormalFlashMaxAlpha = 0.34f;
    private const float FinalFlashMaxAlpha = 0.58f;
    private const float NormalLineMaxAlpha = 0.74f;
    private const float FinalLineMaxAlpha = 0.95f;
    private const float HorizontalLineRatio = 0.82f;
    private const float VerticalLineRatio = 0.50f;

    private static readonly Color NormalFlashColor = new(1.0f, 0.70f, 0.18f, 0.0f);
    private static readonly Color FinalFlashColor = new(0.95f, 0.08f, 0.20f, 0.0f);
    private static readonly Color LineColor = new(1.0f, 0.78f, 0.22f, 0.0f);

    private readonly int _shotNumber;
    private readonly bool _isFinalShot;
    private readonly double _duration;

    private ColorRect? _flash;
    private ColorRect? _horizontalLine;
    private ColorRect? _verticalLine;
    private ColorRect? _markerLine;
    private double _elapsed;

    public CurtainCallShotVfxNode(int shotNumber, bool isFinalShot)
    {
        _shotNumber = Math.Max(1, shotNumber);
        _isFinalShot = isFinalShot;
        _duration = isFinalShot ? FinalDuration : NormalDuration;
    }

    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Always;

        Vector2 viewportSize = JhinVfx.GetViewportSize(this);
        Vector2 center = viewportSize / 2.0f;
        float markerOffset = (_shotNumber - 2.5f) * 12.0f;

        _flash = JhinVfx.CreateFullScreenRect(_isFinalShot ? FinalFlashColor : NormalFlashColor);
        _horizontalLine = JhinVfx.CreateLine(
            LineColor,
            center,
            new Vector2(viewportSize.X * HorizontalLineRatio, _isFinalShot ? 4.0f : 2.0f));
        _verticalLine = JhinVfx.CreateLine(
            LineColor,
            center,
            new Vector2(_isFinalShot ? 4.0f : 2.0f, viewportSize.Y * VerticalLineRatio));
        _markerLine = JhinVfx.CreateLine(
            LineColor,
            new Vector2(center.X + markerOffset, center.Y),
            new Vector2(1.0f, _isFinalShot ? 52.0f : 34.0f));

        AddChild(_flash);
        AddChild(_horizontalLine);
        AddChild(_verticalLine);
        AddChild(_markerLine);
    }

    public override void _Process(double delta)
    {
        _elapsed += delta;
        if (_elapsed >= _duration)
        {
            QueueFree();
            return;
        }

        double normalized = _elapsed / _duration;
        double fadeOut = 1.0d - JhinVfx.SmoothStep(0.08d, 1.0d, normalized);
        float flashAlpha = GetFlashAlpha();
        float lineAlpha = (_isFinalShot ? FinalLineMaxAlpha : NormalLineMaxAlpha) * (float)fadeOut;

        JhinVfx.SetAlpha(_flash, flashAlpha);
        JhinVfx.SetAlpha(_horizontalLine, lineAlpha);
        JhinVfx.SetAlpha(_verticalLine, lineAlpha * 0.72f);
        JhinVfx.SetAlpha(_markerLine, lineAlpha);
    }

    private float GetFlashAlpha()
    {
        float maxAlpha = _isFinalShot ? FinalFlashMaxAlpha : NormalFlashMaxAlpha;
        if (_elapsed <= FlashPeakTime)
        {
            return maxAlpha * (float)JhinVfx.SmoothStep(0.0d, FlashPeakTime, _elapsed);
        }

        return maxAlpha * (float)(1.0d - JhinVfx.SmoothStep(FlashPeakTime, _duration, _elapsed));
    }
}
