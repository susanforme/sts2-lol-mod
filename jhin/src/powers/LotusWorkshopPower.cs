#nullable enable

using BaseLib.Abstracts;
using BaseLib.Patches.Localization;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using jhin.Actions;

namespace jhin.Powers;

public class LotusWorkshopPower : CustomPowerModel, IAddDumbVariablesToPowerDescription
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public void AddDumbVariablesToPowerDescription(LocString description)
    {
        description.Add("trapAmount", Amount > 1 ? 2 : 1);
    }

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(Cards.JhinKeywords.Mark),
        HoverTipFactory.FromKeyword(Cards.JhinKeywords.LotusTrap),
    ];

    public static void TryTrigger(Creature? target)
    {
        if (target?.CombatState is null || !target.IsAlive)
        {
            return;
        }

        foreach (Creature playerCreature in target.CombatState.PlayerCreatures)
        {
            if (!playerCreature.IsAlive)
            {
                continue;
            }

            LotusWorkshopPower? power = playerCreature.GetPower<LotusWorkshopPower>();
            if (power is not null)
            {
                power.Flash();
                int trapAmount = power.Amount > 1 ? 2 : 1;
                _ = ApplyLotusTrapAction.Execute(target, trapAmount);
                break;
            }
        }
    }
}
