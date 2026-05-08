#nullable enable

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Creatures;
using jhin.Magazine;
using jhin.Powers;
using jhin.Relics;

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
    public static int GetMarkAmount(Creature? target)
    {
        return target?.GetPower<MarkPower>()?.Amount ?? 0;
    }

    public static void ConsumeMarks(Creature? target, MegaCrit.Sts2.Core.Entities.Players.Player? player = null)
    {
        if (target is null)
        {
            return;
        }

        MarkPower? markPower = target.GetPower<MarkPower>();
        if (markPower is null)
        {
            return;
        }

        int consumedCount = markPower.Amount;
        _ = PowerCmd.Remove(markPower);

        if (player is not null && consumedCount > 0)
        {
            MasterpieceBornPower.OnMarkConsumed(player);

            if (!target.IsAlive && player.GetRelic<Relics.ShowTicket>() is not null)
            {
                player.Gold += 6;
                player.GetRelic<Relics.ShowTicket>()!.Flash();
            }
        }
    }

    /// <summary>
    /// Attempts to consume one bullet. Returns the result indicating whether
    /// the shot was normal, triggered a flourish, or failed.
    /// </summary>
    public static ShootResult Execute(Player? player)
    {
        if (player is null)
        {
            return ShootResult.Failed;
        }

        JhinMagazineState? state = JhinMagazineStateRegistry.TryGet(player);
        if (state is null || !state.CanShoot())
        {
            return ShootResult.Failed;
        }

        bool isFlourish = state.TryConsumeBulletAndCheckFlourish();
        if (state.Bullets == 0)
        {
            BulletEmptyEventBus.Notify(player, state);
        }

        return isFlourish ? ShootResult.Flourish : ShootResult.Normal;
    }
}
