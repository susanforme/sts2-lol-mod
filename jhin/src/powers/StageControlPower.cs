#nullable enable

using BaseLib.Abstracts;
using BaseLib.Patches.Localization;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using jhin.Actions;

namespace jhin.Powers;

/// <summary>
/// 控场艺术 / Stage Control — When applying Weak to an enemy, also apply 1 Mark. Upgrade: 2 Mark.
/// </summary>
public class StageControlPower : CustomPowerModel, IAddDumbVariablesToPowerDescription
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(Cards.JhinKeywords.Mark),
    ];

    public void AddDumbVariablesToPowerDescription(LocString description)
    {
        description.Add("markAmount", Amount > 1 ? 2 : 1);
    }

    public static void TryApplyMarkOnWeak(Creature target, Creature? weakSource)
    {
        if (target is null || !target.IsAlive || weakSource?.CombatState is null)
        {
            return;
        }

        foreach (Creature playerCreature in weakSource.CombatState.PlayerCreatures)
        {
            if (!playerCreature.IsAlive)
            {
                continue;
            }

            StageControlPower? stageControl = playerCreature.GetPower<StageControlPower>();
            if (stageControl is not null)
            {
                int markAmount = stageControl.Amount > 1 ? 2 : 1;
                stageControl.Flash();
                _ = ApplyMarkAction.Execute(target, markAmount);
                break;
            }
        }
    }
}
