#nullable enable

using MegaCrit.Sts2.Core.Entities.Players;
using jhin.Magazine;

namespace jhin.Actions;

public static class ShootAction
{
    public static bool Execute(Player? player)
    {
        JhinMagazineState? state = JhinMagazineStateRegistry.TryGet(player);
        return state is not null && state.TryConsumeBullet();
    }
}
