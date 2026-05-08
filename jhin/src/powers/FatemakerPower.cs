#nullable enable

using BaseLib.Abstracts;
using BaseLib.Patches.Localization;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using jhin.Actions;

namespace jhin.Powers;

public class FatemakerPower : CustomPowerModel, IAddDumbVariablesToPowerDescription, IJhinTurnStartPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(Cards.JhinKeywords.Bullet),
    ];

    public void AddDumbVariablesToPowerDescription(LocString description)
    {
        description.Add("damage", Amount > 1 ? 9 : 6);
    }

    public void OnTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner?.Player || Owner is null || !Owner.IsAlive)
        {
            return;
        }

        TaskHelper.RunSafely(FireTurnStartShot(choiceContext, player));
    }

    private async Task FireTurnStartShot(PlayerChoiceContext choiceContext, Player player)
    {
        bool fired = await JhinCombatActionUtil.ExecuteRandomEnemyShoot(choiceContext, player, Amount > 1 ? 9 : 6);
        if (fired)
        {
            Flash();
        }
    }
}
