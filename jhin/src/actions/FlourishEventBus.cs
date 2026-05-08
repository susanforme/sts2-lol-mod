#nullable enable

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using jhin.Magazine;

namespace jhin.Actions;

/// <summary>
/// Lightweight event bus for flourish notifications.
/// Listeners (relics, powers, ability cards) subscribe here to react when
/// a flourish is triggered.
/// </summary>
public static class FlourishEventBus
{
    public delegate void FlourishTriggeredHandler(PlayerChoiceContext choiceContext, Player player, JhinMagazineState state);

    /// <summary>
    /// Fired after the last bullet is consumed and flourish is confirmed.
    /// The card's own OnFlourish effect is called before this event.
    /// </summary>
    public static event FlourishTriggeredHandler? OnFlourishTriggered;

    internal static void Notify(PlayerChoiceContext choiceContext, Player player, JhinMagazineState state)
    {
        OnFlourishTriggered?.Invoke(choiceContext, player, state);
    }

    /// <summary>
    /// Clears all listeners. Called at combat end to prevent stale references.
    /// </summary>
    public static void ClearListeners()
    {
        OnFlourishTriggered = null;
    }
}
