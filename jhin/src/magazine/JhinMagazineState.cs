#nullable enable

using MegaCrit.Sts2.Core.Entities.Players;
using jhin.Powers;

namespace jhin.Magazine;

public sealed class JhinMagazineState
{
    private bool _flourishDisabledThisTurn;
    private bool _forceNextShotFlourish;

    public int Bullets { get; private set; }
    public int MaxBullets { get; private set; }
    public int BaseMaxBullets => 4;
    public int FlourishCountThisTurn { get; private set; }
    public int FlourishCountThisCombat { get; private set; }
    public bool UsedShootThisTurn { get; private set; }
    public bool UsedSkillThisTurn { get; private set; }
    public bool UsedNonShootAttackThisTurn { get; private set; }
    public int AttackCardCountThisTurn { get; private set; }
    public BulletPower? AppliedPower { get; private set; }
    public bool HasForcedFlourish => _forceNextShotFlourish;
    public bool HasTriggeredFlourishThisTurn => FlourishCountThisTurn > 0;

    public void InitializeCombat()
    {
        MaxBullets = 4;
        Bullets = MaxBullets;
        FlourishCountThisTurn = 0;
        FlourishCountThisCombat = 0;
        UsedShootThisTurn = false;
        UsedSkillThisTurn = false;
        UsedNonShootAttackThisTurn = false;
        AttackCardCountThisTurn = 0;
        _flourishDisabledThisTurn = false;
        _forceNextShotFlourish = false;
        SyncPower();
    }

    public void IncreaseMaxBullets(int amount)
    {
        MaxBullets = BaseMaxBullets + amount;
        if (Bullets > MaxBullets)
        {
            Bullets = MaxBullets;
        }

        SyncPowerForce();
    }

    public void StartTurn()
    {
        bool reloaded = Bullets == 0;
        if (reloaded)
        {
            Bullets = MaxBullets;
        }

        FlourishCountThisTurn = 0;
        UsedShootThisTurn = false;
        UsedSkillThisTurn = false;
        UsedNonShootAttackThisTurn = false;
        AttackCardCountThisTurn = 0;
        _flourishDisabledThisTurn = false;

        if (reloaded)
        {
            SyncPowerForce();
        }
        else
        {
            SyncPower();
        }
    }

    public bool CanShoot()
    {
        return Bullets > 0;
    }

    public bool WouldFlourishOnNextShot()
    {
        return Bullets > 0 && !_flourishDisabledThisTurn && (Bullets == 1 || _forceNextShotFlourish);
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

    /// <summary>
    /// Consumes one bullet and returns whether this was the last bullet (flourish triggered).
    /// </summary>
    public bool TryConsumeBulletAndCheckFlourish()
    {
        if (Bullets <= 0)
        {
            return false;
        }

        bool wasLastBullet = Bullets == 1;
        bool forcedFlourish = _forceNextShotFlourish && !_flourishDisabledThisTurn;
        Bullets--;
        UsedShootThisTurn = true;
        _forceNextShotFlourish = false;

        bool didFlourish = (wasLastBullet || forcedFlourish) && !_flourishDisabledThisTurn;

        if (didFlourish)
        {
            FlourishCountThisTurn++;
            FlourishCountThisCombat++;
        }

        SyncPower();
        return didFlourish;
    }

    public void ReloadToFull()
    {
        Bullets = MaxBullets;
        SyncPowerForce();
    }

    public void RecordSkillPlayed()
    {
        UsedSkillThisTurn = true;
    }

    public void RecordNonShootAttackPlayed()
    {
        UsedNonShootAttackThisTurn = true;
    }

    public void RecordAttackCardPlayed()
    {
        AttackCardCountThisTurn++;
    }

    public void DisableFlourishThisTurn()
    {
        _flourishDisabledThisTurn = true;
        _forceNextShotFlourish = false;
    }

    public void ForceNextShotFlourish()
    {
        _forceNextShotFlourish = true;
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

    /// <summary>
    /// Forces a Power notification even if the Amount hasn't changed,
    /// ensuring CombatStateTracker picks up the state change.
    /// </summary>
    private void SyncPowerForce()
    {
        if (AppliedPower is null) return;
        // Temporarily set a different value (silent) so the real SetAmount always has delta != 0.
        if (AppliedPower.Amount == Bullets)
        {
            AppliedPower.SetAmount(Bullets > 0 ? Bullets - 1 : Bullets + 1, silent: true);
        }
        AppliedPower.SyncFrom(this);
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

    public static Player? GetPlayerFromCombatState(PlayerCombatState? combatState)
    {
        if (combatState is null)
        {
            return null;
        }

        PlayersByCombatState.TryGetValue(combatState, out Player? player);
        return player;
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
