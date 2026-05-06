#nullable enable

using BaseLib.Abstracts;
using BaseLib.Patches.Localization;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.ValueProps;
using jhin.Actions;
using jhin.Magazine;

namespace jhin.Powers;

/// <summary>
/// 演出计划 / Show Plan — On reload: gain 3 Block. Upgrade: 5 Block.
/// Subscription managed externally (subscribed from card OnPlay, unsubscribed at combat end).
/// </summary>
public class ShowPlanPower : CustomPowerModel, IAddDumbVariablesToPowerDescription
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
        description.Add("blockAmount", Amount > 1 ? 5 : 3);
    }

    public void SubscribeEvents()
    {
        ReloadEventBus.OnReloadTriggered += OnReloadTriggered;
    }

    private void OnReloadTriggered(Player player, JhinMagazineState state, int bulletsBeforeReload)
    {
        if (player != Owner?.Player || Owner?.Player?.Creature is null)
        {
            return;
        }

        int blockAmount = Amount > 1 ? 5 : 3;
        Flash();
        _ = CreatureCmd.GainBlock(Owner, blockAmount, ValueProp.Move, null, false);
    }
}
