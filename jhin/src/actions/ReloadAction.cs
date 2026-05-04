#nullable enable

using MegaCrit.Sts2.Core.Entities.Players;
using jhin.Magazine;

namespace jhin.Actions;

public static class ReloadAction
{
    public static void Execute(Player? player, bool onlyWhenEmpty = false)
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

        state.ReloadToFull();
    }
}
