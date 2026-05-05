#nullable enable

using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using jhin.Powers;

namespace jhin.Actions;

public static class ApplyMarkAction
{
    public static bool Execute(Creature? target, int amount)
    {
        if (target is null || amount <= 0 || !target.IsAlive || !target.CanReceivePowers)
        {
            return false;
        }

        MarkPower? existingPower = target.GetPower<MarkPower>();
        if (existingPower is not null)
        {
            existingPower.AddStacks(amount);
        }
        else
        {
            MarkPower markPower = (MarkPower)ModelDb.Power<MarkPower>().ToMutable();
            markPower.ApplyInternal(target, amount, silent: false);
        }

        LotusWorkshopPower.TryTrigger(target);
        return true;
    }

    public static Creature? ExecuteRandomEnemy(Player? player, int amount)
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

        return Execute(target, amount) ? target : null;
    }
}
