#nullable enable

using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using jhin.Powers;

namespace jhin.Actions;

public static class ApplyMarkAction
{
    public static async Task<bool> Execute(Creature? target, int amount)
    {
        if (target is null || amount <= 0 || !target.IsAlive || !target.CanReceivePowers)
        {
            return false;
        }

        await CommonActions.Apply<MarkPower>(new ThrowingPlayerChoiceContext(), target, null, amount);

        LotusWorkshopPower.TryTrigger(target);
        return true;
    }

    public static async Task<Creature?> ExecuteRandomEnemy(Player? player, int amount)
    {
        if (player?.Creature?.CombatState is null || amount <= 0)
        {
            return null;
        }

        List<Creature> enemies = player.Creature.CombatState.HittableEnemies
            .Where(enemy => enemy.IsAlive)
            .ToList();

        if (enemies.Count == 0)
        {
            return null;
        }

        Creature? target = player.PlayerRng.Transformations.NextItem(enemies);
        if (target is null)
        {
            return null;
        }

        return await Execute(target, amount) ? target : null;
    }
}
