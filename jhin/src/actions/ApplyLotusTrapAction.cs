#nullable enable

using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using jhin.Powers;

namespace jhin.Actions;

public static class ApplyLotusTrapAction
{
    public static async Task<bool> Execute(Creature? target, int amount)
    {
        if (target is null || amount <= 0 || !target.IsAlive || !target.CanReceivePowers)
        {
            return false;
        }

        await CommonActions.Apply<LotusTrapPower>(new ThrowingPlayerChoiceContext(), target, null, amount);
        return true;
    }

    public static async Task<int> ExecuteAllEnemies(Player? player, int amount)
    {
        if (player?.Creature?.CombatState is null || amount <= 0)
        {
            return 0;
        }

        int appliedCount = 0;
        foreach (Creature enemy in player.Creature.CombatState.HittableEnemies.Where(enemy => enemy.IsAlive))
        {
            if (await Execute(enemy, amount))
            {
                appliedCount++;
            }
        }

        return appliedCount;
    }
}
