#nullable enable

using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using jhin.Actions;
using jhin.Magazine;

namespace jhin.Powers;

public class AestheticOfFourPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public void OnTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        JhinMagazineState? state = JhinMagazineStateRegistry.TryGet(player);
        if (state is null || state.Bullets != 4)
        {
            return;
        }

        Flash();
        _ = PlayerCmd.GainEnergy(1m, player);

        if (Amount > 1)
        {
            _ = JhinCombatActionUtil.Draw(choiceContext, player, 1);
        }
    }
}
