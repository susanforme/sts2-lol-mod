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

public class MasterpieceBornPower : CustomPowerModel, IAddDumbVariablesToPowerDescription
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public void AddDumbVariablesToPowerDescription(LocString description)
    {
        description.Add("drawAmount", Amount > 1 ? 2 : 1);
    }

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(Cards.JhinKeywords.Mark),
    ];

    public static void OnMarkConsumed(Player player)
    {
        if (player?.Creature is null || !player.Creature.IsAlive)
        {
            return;
        }

        MasterpieceBornPower? power = player.Creature.GetPower<MasterpieceBornPower>();
        if (power is null)
        {
            return;
        }

        power.Flash();
        int drawAmount = power.Amount > 1 ? 2 : 1;
        _ = JhinCombatActionUtil.Draw(null!, player, drawAmount);
    }
}
