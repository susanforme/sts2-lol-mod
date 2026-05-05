#nullable enable

using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using jhin.Magazine;

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
        return (JhinMagazineStateRegistry.TryGet(player)?.Bullets ?? 0) == 1;
    }

    public static void DisableFlourishThisTurn(Player? player)
    {
        JhinMagazineStateRegistry.TryGet(player)?.DisableFlourishThisTurn();
    }

    public static void ApplyOrStackVulnerable(Creature? target, int amount)
    {
        if (target is null || amount <= 0 || !target.IsAlive || !target.CanReceivePowers)
        {
            return;
        }

        VulnerablePower? existingPower = target.GetPower<VulnerablePower>();
        if (existingPower is not null)
        {
            existingPower.SetAmount(existingPower.Amount + amount, silent: false);
            return;
        }

        VulnerablePower vulnerablePower = (VulnerablePower)ModelDb.Power<VulnerablePower>().ToMutable();
        vulnerablePower.ApplyInternal(target, amount, silent: false);
    }
}
