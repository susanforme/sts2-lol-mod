#nullable enable

using BaseLib.Abstracts;
using BaseLib.Patches.Localization;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using jhin.Actions;
using jhin.Utils;
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

    public static void CheckAfterCardPlayed(CardPlay cardPlay, Creature creature)
    {
        if (creature.CombatState is null) return;

        DeathIsArtPower? power = creature.GetPower<DeathIsArtPower>();
        if (power is null) return;

        EnemyThresholdTriggerUtil.TriggerOncePerEnemyBelowHpThreshold(
            creature.CombatState.HittableEnemies,
            power._triggeredEnemies,
            0.5m,
            enemy =>
            {
                power.Flash();
                int strAmount = power.Amount > 1 ? 2 : 1;
                _ = JhinCombatActionUtil.ApplyOrStackStrength(creature, strAmount);
            });
    }
}
