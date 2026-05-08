#nullable enable

using MegaCrit.Sts2.Core.Entities.Players;
using jhin.Powers;

namespace jhin.Magazine;

[Flags]
public enum MagazineStateChange
{
    None = 0,
    Bullets = 1 << 0,
    MaxBullets = 1 << 1,
    FlourishState = 1 << 2,
    TurnState = 1 << 3,
    PlayStats = 1 << 4,
}

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
    public int CardsPlayedThisTurn { get; private set; }
    public BulletPower? AppliedPower { get; private set; }
    public bool HasForcedFlourish => _forceNextShotFlourish;
    public bool HasTriggeredFlourishThisTurn => FlourishCountThisTurn > 0;
    public event Action<JhinMagazineState, MagazineStateChange>? StateChanged;

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
        CardsPlayedThisTurn = 0;
        _flourishDisabledThisTurn = false;
        _forceNextShotFlourish = false;
        NotifyStateChanged(MagazineStateChange.Bullets | MagazineStateChange.MaxBullets | MagazineStateChange.FlourishState | MagazineStateChange.TurnState | MagazineStateChange.PlayStats);
    }

    public void IncreaseMaxBullets(int amount)
    {
        int previousBullets = Bullets;
        MaxBullets = BaseMaxBullets + amount;
        if (Bullets > MaxBullets)
        {
            Bullets = MaxBullets;
        }

        MagazineStateChange change = MagazineStateChange.MaxBullets;
        if (Bullets != previousBullets)
        {
            change |= MagazineStateChange.Bullets;
        }

        NotifyStateChanged(change);
    }

    public void SetBulletsToZero()
    {
        Bullets = 0;
        NotifyStateChanged(MagazineStateChange.Bullets);
    }

    public void RestoreBullet()
    {
        if (Bullets < MaxBullets)
        {
            Bullets++;
            NotifyStateChanged(MagazineStateChange.Bullets);
        }
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

        MagazineStateChange change = MagazineStateChange.FlourishState | MagazineStateChange.TurnState | MagazineStateChange.PlayStats;
        if (reloaded)
        {
            change |= MagazineStateChange.Bullets;
        }

        NotifyStateChanged(change);
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
        NotifyStateChanged(MagazineStateChange.Bullets | MagazineStateChange.TurnState);
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

        NotifyStateChanged(MagazineStateChange.Bullets | MagazineStateChange.FlourishState | MagazineStateChange.TurnState);
        return didFlourish;
    }

    public void ReloadToFull()
    {
        Bullets = MaxBullets;
        NotifyStateChanged(MagazineStateChange.Bullets);
    }

    public void RecordSkillPlayed()
    {
        UsedSkillThisTurn = true;
        NotifyStateChanged(MagazineStateChange.PlayStats);
    }

    public void RecordNonShootAttackPlayed()
    {
        UsedNonShootAttackThisTurn = true;
        NotifyStateChanged(MagazineStateChange.PlayStats);
    }

    public void RecordAttackCardPlayed()
    {
        AttackCardCountThisTurn++;
        NotifyStateChanged(MagazineStateChange.PlayStats);
    }

    public void IncrementCardsPlayed()
    {
        CardsPlayedThisTurn++;
        NotifyStateChanged(MagazineStateChange.PlayStats);
    }

    public void DisableFlourishThisTurn()
    {
        _flourishDisabledThisTurn = true;
        _forceNextShotFlourish = false;
        NotifyStateChanged(MagazineStateChange.FlourishState);
    }

    public void ForceNextShotFlourish()
    {
        _forceNextShotFlourish = true;
        NotifyStateChanged(MagazineStateChange.FlourishState);
    }

    public void AttachPower(BulletPower power)
    {
        AppliedPower?.UnsubscribeFromMagazine(this);
        AppliedPower = power;
        AppliedPower.SubscribeToMagazine(this);
    }

    public void DetachPower()
    {
        AppliedPower?.UnsubscribeFromMagazine(this);
        AppliedPower = null;
    }

    private void NotifyStateChanged(MagazineStateChange change)
    {
        if (change != MagazineStateChange.None)
        {
            StateChanged?.Invoke(this, change);
        }
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
