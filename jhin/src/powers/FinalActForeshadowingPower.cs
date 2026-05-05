#nullable enable

using BaseLib.Abstracts;
using BaseLib.Patches.Localization;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;

namespace jhin.Powers;

public class FinalActForeshadowingPower : CustomPowerModel, IAddDumbVariablesToPowerDescription
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public void AddDumbVariablesToPowerDescription(LocString description)
    {
        description.Add("energyAmount", Amount > 1 ? 2 : 1);
    }

    public void OnTurnStart(Player player)
    {
        Flash();
        int energyAmount = Amount > 1 ? 2 : 1;
        _ = PlayerCmd.GainEnergy(energyAmount, player);
    }
}
