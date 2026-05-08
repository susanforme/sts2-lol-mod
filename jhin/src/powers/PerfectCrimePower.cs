#nullable enable

using BaseLib.Abstracts;
using BaseLib.Patches.Localization;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using jhin.Actions;

namespace jhin.Powers;

public class PerfectCrimePower : CustomPowerModel, IAddDumbVariablesToPowerDescription
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public void AddDumbVariablesToPowerDescription(LocString description)
    {
        description.Add("gainAmount", Amount > 1 ? 2 : 1);
    }

    public void OnTurnStart()
    {
        if (Owner is null)
        {
            return;
        }

        Flash();
        int gainAmount = Amount > 1 ? 2 : 1;
        _ = JhinCombatActionUtil.ApplyOrStackStrength(Owner, gainAmount);
    }
}
