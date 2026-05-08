#nullable enable

using MegaCrit.Sts2.Core.Entities.Creatures;

namespace jhin.Utils;

/// <summary>
/// Shared helpers for one-time enemy HP-threshold trigger checks.
/// </summary>
public static class EnemyThresholdTriggerUtil
{
    /// <summary>
    /// Iterates enemies and invokes <paramref name="onTriggered"/> the first time each living enemy drops below the given HP ratio threshold.
    /// Triggered enemies are recorded in <paramref name="triggeredEnemies"/> so they only fire once.
    /// </summary>
    public static void TriggerOncePerEnemyBelowHpThreshold(
        IEnumerable<Creature> enemies,
        HashSet<Creature> triggeredEnemies,
        decimal threshold,
        Action<Creature> onTriggered)
    {
        foreach (Creature enemy in enemies)
        {
            if (!enemy.IsAlive || triggeredEnemies.Contains(enemy) || enemy.MaxHp <= 0)
            {
                continue;
            }

            if ((decimal)enemy.CurrentHp / enemy.MaxHp < threshold)
            {
                triggeredEnemies.Add(enemy);
                onTriggered(enemy);
            }
        }
    }
}
