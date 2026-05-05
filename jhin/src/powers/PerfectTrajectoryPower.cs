#nullable enable

using BaseLib.Abstracts;
using BaseLib.Patches.Localization;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.ValueProps;

namespace jhin.Powers;

public class PerfectTrajectoryPower : CustomPowerModel, IAddDumbVariablesToPowerDescription
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public void AddDumbVariablesToPowerDescription(LocString description)
    {
        description.Add("bonusDamage", Amount > 1 ? 5 : 3);
    }

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(Cards.JhinKeywords.Bullet),
    ];

    public static int GetBonusShootDamage(Creature? playerCreature)
    {
        if (playerCreature is null || !playerCreature.IsAlive)
        {
            return 0;
        }

        PerfectTrajectoryPower? power = playerCreature.GetPower<PerfectTrajectoryPower>();
        if (power is null)
        {
            return 0;
        }

        return power.Amount > 1 ? 5 : 3;
    }
}
