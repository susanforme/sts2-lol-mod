#nullable enable

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using jhin.Magazine;

namespace jhin.Actions;

public static class ReloadEventBus
{
    public delegate void ReloadTriggeredHandler(PlayerChoiceContext choiceContext, Player player, JhinMagazineState state, int bulletsBeforeReload);

    public static event ReloadTriggeredHandler? OnReloadTriggered;

    internal static void Notify(PlayerChoiceContext choiceContext, Player player, JhinMagazineState state, int bulletsBeforeReload)
    {
        OnReloadTriggered?.Invoke(choiceContext, player, state, bulletsBeforeReload);
    }

    public static void ClearListeners()
    {
        OnReloadTriggered = null;
    }
}
