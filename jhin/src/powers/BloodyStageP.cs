#nullable enable

using BaseLib.Abstracts;
using BaseLib.Patches.Localization;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using jhin.Actions;

namespace jhin.Powers;

/// <summary>
/// 血色舞台 / Bloody Stage — When enemy first drops below 50%: apply 2 Mark. Upgrade: 3 Mark.
/// Implemented via AfterCardPlayed: after any attack card, check all enemies for HP threshold.
/// </summary>
public class BloodyStageP : CustomPowerModel, IAddDumbVariablesToPowerDescription
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    private readonly HashSet<int> _triggeredEnemyIds = [];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(Cards.JhinKeywords.Mark),
    ];

    public void AddDumbVariablesToPowerDescription(LocString description)
    {
        description.Add("markAmount", Amount > 1 ? 3 : 2);
    }

    public override Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner?.Player || cardPlay.Card.Type != CardType.Attack)
        {
            return Task.CompletedTask;
        }

        if (Owner?.CombatState is null)
        {
            return Task.CompletedTask;
        }

        foreach (Creature enemy in Owner.CombatState.HittableEnemies.Where(e => e.IsAlive))
        {
            int enemyId = enemy.GetHashCode();
            if (_triggeredEnemyIds.Contains(enemyId))
            {
                continue;
            }

            if (enemy.MaxHp > 0 && (decimal)enemy.CurrentHp / enemy.MaxHp < 0.5m)
            {
                _triggeredEnemyIds.Add(enemyId);
                Flash();
                int markAmount = Amount > 1 ? 3 : 2;
                _ = ApplyMarkAction.Execute(enemy, markAmount);
            }
        }

        return Task.CompletedTask;
    }
}
