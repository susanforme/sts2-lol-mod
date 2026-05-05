#nullable enable

using BaseLib.Abstracts;
using BaseLib.Patches.Localization;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using jhin.Actions;
using System.Collections.Generic;

namespace jhin.Powers;

public class DeathIsArtPower : CustomPowerModel, IAddDumbVariablesToPowerDescription
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public void AddDumbVariablesToPowerDescription(LocString description)
    {
        description.Add("strengthAmount", Amount > 1 ? 2 : 1);
    }

    private readonly HashSet<Creature> _triggeredEnemies = [];

    public void AfterCardPlayed(CardPlay cardPlay)
    {
        if (Owner?.CombatState is null)
        {
            return;
        }

        foreach (Creature enemy in Owner.CombatState.HittableEnemies)
        {
            if (!enemy.IsAlive || _triggeredEnemies.Contains(enemy))
            {
                continue;
            }

            if (enemy.MaxHp > 0 && (decimal)enemy.CurrentHp / enemy.MaxHp < 0.5m)
            {
                _triggeredEnemies.Add(enemy);
                Flash();
                int strAmount = Amount > 1 ? 2 : 1;
                JhinCombatActionUtil.ApplyOrStackStrength(Owner, strAmount);
            }
        }
    }
}
