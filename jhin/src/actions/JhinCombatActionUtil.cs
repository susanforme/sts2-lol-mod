#nullable enable

using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Entities.Powers;
using jhin.Magazine;
using jhin.Powers;

namespace jhin.Actions;

public static class JhinCombatActionUtil
{
    public static Task Draw(PlayerChoiceContext choiceContext, Player? player, int amount)
    {
        if (player is null || amount <= 0)
        {
            return Task.CompletedTask;
        }

        return CardPileCmd.Draw(choiceContext, amount, player);
    }

    public static Task GainEnergy(Player? player, int amount)
    {
        if (player is null || amount <= 0)
        {
            return Task.CompletedTask;
        }

        return PlayerCmd.GainEnergy(amount, player);
    }

    public static bool HasShotThisTurn(Player? player)
    {
        return JhinMagazineStateRegistry.TryGet(player)?.UsedShootThisTurn ?? false;
    }

    public static bool HasBulletCount(Player? player, params int[] bulletCounts)
    {
        JhinMagazineState? state = JhinMagazineStateRegistry.TryGet(player);
        if (state is null)
        {
            return false;
        }

        return bulletCounts.Contains(state.Bullets);
    }

    public static bool IsFlourishBullet(Player? player)
    {
        return JhinMagazineStateRegistry.TryGet(player)?.WouldFlourishOnNextShot() ?? false;
    }

    public static void DisableFlourishThisTurn(Player? player)
    {
        JhinMagazineStateRegistry.TryGet(player)?.DisableFlourishThisTurn();
    }

    public static bool HasForcedFlourish(Player? player)
    {
        return JhinMagazineStateRegistry.TryGet(player)?.HasForcedFlourish ?? false;
    }

    public static void ForceNextShotFlourish(Player? player)
    {
        JhinMagazineStateRegistry.TryGet(player)?.ForceNextShotFlourish();
    }

    public static async Task ApplyOrStackVulnerable(Creature? target, int amount)
    {
        if (target is null || amount <= 0 || !target.IsAlive || !target.CanReceivePowers)
        {
            return;
        }

        await CommonActions.Apply<VulnerablePower>(new ThrowingPlayerChoiceContext(), target, null, amount);
    }

    public static async Task ApplyOrStackWeak(Creature? target, int amount, Creature? weakSource = null)
    {
        if (target is null || amount <= 0 || !target.IsAlive || !target.CanReceivePowers)
        {
            return;
        }

        await CommonActions.Apply<WeakPower>(new ThrowingPlayerChoiceContext(), target, null, amount);

        StageControlPower.TryApplyMarkOnWeak(target, weakSource);
    }

    public static async Task ApplyOrStackStrength(Creature? target, int amount)
    {
        if (target is null || amount <= 0 || !target.IsAlive || !target.CanReceivePowers)
        {
            return;
        }

        await CommonActions.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), target, null, amount);
    }

    public static async Task ApplyOrStackDexterity(Creature? target, int amount)
    {
        if (target is null || amount <= 0 || !target.IsAlive || !target.CanReceivePowers)
        {
            return;
        }

        await CommonActions.Apply<DexterityPower>(new ThrowingPlayerChoiceContext(), target, null, amount);
    }

    public static bool HasPlayedSkillThisTurn(Player? player)
    {
        return JhinMagazineStateRegistry.TryGet(player)?.UsedSkillThisTurn ?? false;
    }

    public static bool HasPlayedNonShootAttackThisTurn(Player? player)
    {
        return JhinMagazineStateRegistry.TryGet(player)?.UsedNonShootAttackThisTurn ?? false;
    }

    public static int GetAttackCardCountThisTurn(Player? player)
    {
        return JhinMagazineStateRegistry.TryGet(player)?.AttackCardCountThisTurn ?? 0;
    }

    public static void RecordSkillPlayed(Player? player)
    {
        JhinMagazineStateRegistry.TryGet(player)?.RecordSkillPlayed();
    }

    public static void RecordNonShootAttackPlayed(Player? player)
    {
        JhinMagazineStateRegistry.TryGet(player)?.RecordNonShootAttackPlayed();
    }

    public static void RecordAttackCardPlayed(Player? player)
    {
        JhinMagazineStateRegistry.TryGet(player)?.RecordAttackCardPlayed();
    }
}
