#nullable enable

using BaseLib.Abstracts;
using BaseLib.Patches.Localization;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.ValueProps;
using jhin.Actions;
using jhin.Magazine;

namespace jhin.Powers;

/// <summary>
/// 枪械保养 / Gun Maintenance — On play a reload card: draw 1. Upgrade: draw 1 + 2 Block.
/// Subscription managed externally (subscribed from card OnPlay, unsubscribed at combat end).
/// </summary>
public class GunMaintenancePower : CustomPowerModel, IAddDumbVariablesToPowerDescription
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(Cards.JhinKeywords.Bullet),
        HoverTipFactory.FromKeyword(Cards.JhinKeywords.Reload),
    ];

    public void AddDumbVariablesToPowerDescription(LocString description)
    {
        description.Add("blockAmount", 2);
    }

    public void SubscribeEvents()
    {
        ReloadEventBus.OnReloadTriggered += OnReloadTriggered;
    }

    private void OnReloadTriggered(PlayerChoiceContext choiceContext, Player player, JhinMagazineState state, int bulletsBeforeReload)
    {
        if (player != Owner?.Player)
        {
            return;
        }

        Flash();
        _ = JhinCombatActionUtil.Draw(choiceContext, player, 1);

        if (Amount > 1 && Owner is not null)
        {
            _ = CreatureCmd.GainBlock(Owner, 2, ValueProp.Move, null, false);
        }
    }
}
