#nullable enable

using MegaCrit.Sts2.Core.Entities.Players;
using jhin.Magazine;

namespace jhin.Actions;

public enum ShootResult
{
    /// <summary>No bullet was consumed (magazine empty or state unavailable).</summary>
    Failed,
    /// <summary>Bullet consumed, but not the last one — no flourish.</summary>
    Normal,
    /// <summary>Last bullet consumed — flourish triggered.</summary>
    Flourish,
}

public static class ShootAction
{
    /// <summary>
    /// Attempts to consume one bullet. Returns the result indicating whether
    /// the shot was normal, triggered a flourish, or failed.
    /// </summary>
    public static ShootResult Execute(Player? player)
    {
        JhinMagazineState? state = JhinMagazineStateRegistry.TryGet(player);
        if (state is null || !state.CanShoot())
        {
            return ShootResult.Failed;
        }

        bool isFlourish = state.TryConsumeBulletAndCheckFlourish();
        return isFlourish ? ShootResult.Flourish : ShootResult.Normal;
    }
}
