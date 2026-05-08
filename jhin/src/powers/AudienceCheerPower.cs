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

public class AudienceCheerPower : CustomPowerModel, IAddDumbVariablesToPowerDescription
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public void AddDumbVariablesToPowerDescription(LocString description)
    {
        description.Add("dexAmount", Amount > 1 ? 2 : 1);
    }

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(Cards.JhinKeywords.Flourish),
    ];

    public void SubscribeEvents()
    {
        FlourishEventBus.OnFlourishTriggered += OnFlourishTriggered;
    }

    private void OnFlourishTriggered(Player player, JhinMagazineState state)
    {
        if (player != Owner?.Player)
        {
            return;
        }

        Flash();
        int dexAmount = Amount > 1 ? 2 : 1;
        _ = JhinCombatActionUtil.ApplyOrStackDexterity(Owner!, dexAmount);
    }
}
