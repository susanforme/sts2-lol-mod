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

public class CarefulArrangementPower : CustomPowerModel, IAddDumbVariablesToPowerDescription
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public void AddDumbVariablesToPowerDescription(LocString description)
    {
        description.Add("drawAmount", Amount > 1 ? 2 : 1);
    }

    public void OnTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        Flash();
        int drawAmount = Amount > 1 ? 2 : 1;
        _ = JhinCombatActionUtil.Draw(choiceContext, player, drawAmount);
    }
}
