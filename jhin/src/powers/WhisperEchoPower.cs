#nullable enable

using BaseLib.Abstracts;
using BaseLib.Patches.Localization;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using jhin.Actions;
using jhin.Magazine;

namespace jhin.Powers;

public class WhisperEchoPower : CustomPowerModel, IAddDumbVariablesToPowerDescription
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(Cards.JhinKeywords.Flourish),
    ];

    public void AddDumbVariablesToPowerDescription(LocString description)
    {
        description.Add("energyAmount", Amount > 1 ? 2 : 1);
        description.Add("drawAmount", Amount > 1 ? 2 : 1);
    }

    public void SubscribeEvents()
    {
        FlourishEventBus.OnFlourishTriggered += OnFlourishTriggered;
    }

    private void OnFlourishTriggered(PlayerChoiceContext choiceContext, Player player, JhinMagazineState state)
    {
        if (player != Owner?.Player)
        {
            return;
        }

        Flash();
        int energyAmount = Amount > 1 ? 2 : 1;
        int drawAmount = Amount > 1 ? 2 : 1;
        _ = PlayerCmd.GainEnergy(energyAmount, player);
        _ = JhinCombatActionUtil.Draw(choiceContext, player, drawAmount);
    }
}
