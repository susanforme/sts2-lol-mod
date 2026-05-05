#nullable enable

using BaseLib.Abstracts;
using BaseLib.Patches.Localization;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;

namespace jhin.Powers;

public class FinalActArtPower : CustomPowerModel, IAddDumbVariablesToPowerDescription
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public void AddDumbVariablesToPowerDescription(LocString description)
    {
        description.Add("energyAmount", Amount > 1 ? 3 : 2);
    }

    public void OnTurnStart(Player player)
    {
        if (player.Creature?.CombatState is null)
        {
            return;
        }

        float threshold = Amount > 1 ? 0.5f : 0.3f;
        foreach (Creature enemy in player.Creature.CombatState.HittableEnemies)
        {
            if (enemy.IsAlive && enemy.MaxHp > 0 && (decimal)enemy.CurrentHp / enemy.MaxHp < (decimal)threshold)
            {
                Flash();
                int energyAmount = Amount > 1 ? 3 : 2;
                _ = PlayerCmd.GainEnergy(energyAmount, player);
                break;
            }
        }
    }
}
