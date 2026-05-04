#nullable enable

using MegaCrit.Sts2.Core.Entities.Players;
using jhin.Powers;

namespace jhin.Magazine;

public sealed class JhinMagazineState
{
    public int Bullets { get; private set; }
    public int MaxBullets { get; private set; }
    public int FlourishCountThisTurn { get; private set; }
    public int FlourishCountThisCombat { get; private set; }
    public bool UsedShootThisTurn { get; private set; }
    public BulletPower? AppliedPower { get; private set; }

    public void InitializeCombat()
    {
        MaxBullets = 4;
        Bullets = MaxBullets;
        FlourishCountThisTurn = 0;
        FlourishCountThisCombat = 0;
        UsedShootThisTurn = false;
        SyncPower();
    }

    public void StartTurn()
    {
        if (Bullets == 0)
        {
            Bullets = MaxBullets;
        }

        FlourishCountThisTurn = 0;
        UsedShootThisTurn = false;
        SyncPower();
    }

    public bool CanShoot()
    {
        return Bullets > 0;
    }

    public bool TryConsumeBullet()
    {
        if (Bullets <= 0)
        {
            return false;
        }

        Bullets--;
        UsedShootThisTurn = true;
        SyncPower();
        return true;
    }

    public void ReloadToFull()
    {
        Bullets = MaxBullets;
        SyncPower();
    }

    public void AttachPower(BulletPower power)
    {
        AppliedPower = power;
        SyncPower();
    }

    public void DetachPower()
    {
        AppliedPower = null;
    }

    private void SyncPower()
    {
        AppliedPower?.SyncFrom(this);
    }
}

public static class JhinMagazineStateRegistry
{
    private static readonly Dictionary<Player, JhinMagazineState> States = [];
    private static readonly Dictionary<PlayerCombatState, Player> PlayersByCombatState = [];

    public static bool IsJhin(Player? player)
    {
        return player?.Character is Characters.JhinCharacter;
    }

    public static JhinMagazineState? TryGet(Player? player)
    {
        if (player is null)
        {
            return null;
        }

        States.TryGetValue(player, out JhinMagazineState? state);
        return state;
    }

    public static JhinMagazineState GetOrCreate(Player player)
    {
        if (!States.TryGetValue(player, out JhinMagazineState? state))
        {
            state = new JhinMagazineState();
            States[player] = state;
        }

        if (player.PlayerCombatState is not null)
        {
            PlayersByCombatState[player.PlayerCombatState] = player;
        }

        return state;
    }

    public static JhinMagazineState? TryGet(PlayerCombatState? combatState)
    {
        if (combatState is null)
        {
            return null;
        }

        return PlayersByCombatState.TryGetValue(combatState, out Player? player)
            ? TryGet(player)
            : null;
    }

    public static void Clear(Player player)
    {
        States.Remove(player);
        if (player.PlayerCombatState is not null)
        {
            PlayersByCombatState.Remove(player.PlayerCombatState);
        }
    }
}
