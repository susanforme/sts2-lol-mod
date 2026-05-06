#nullable enable

using MegaCrit.Sts2.Core.Entities.Players;
using jhin.Magazine;

namespace jhin.Actions;

public static class ReloadEventBus
{
    public delegate void ReloadTriggeredHandler(Player player, JhinMagazineState state, int bulletsBeforeReload);

    public static event ReloadTriggeredHandler? OnReloadTriggered;

    internal static void Notify(Player player, JhinMagazineState state, int bulletsBeforeReload)
    {
        OnReloadTriggered?.Invoke(player, state, bulletsBeforeReload);
    }

    public static void ClearListeners()
    {
        OnReloadTriggered = null;
    }
}
