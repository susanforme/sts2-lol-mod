#nullable enable

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using jhin.Magazine;

namespace jhin.Actions;

public static class ReloadAction
{
    public static void Execute(PlayerChoiceContext choiceContext, Player? player, bool onlyWhenEmpty = false)
    {
        JhinMagazineState? state = JhinMagazineStateRegistry.TryGet(player);
        if (state is null)
        {
            return;
        }

        if (onlyWhenEmpty && state.Bullets > 0)
        {
            return;
        }

        int bulletsBeforeReload = state.Bullets;
        state.ReloadToFull();

        if (player is not null)
        {
            ReloadEventBus.Notify(choiceContext, player, state, bulletsBeforeReload);
        }
    }
}
