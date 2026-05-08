#nullable enable

using BaseLib.Abstracts;
using BaseLib.Patches.Localization;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using jhin.Actions;
using jhin.Magazine;

namespace jhin.Powers;

/// <summary>
/// 演员本能 / Actor's Instinct — If no flourish this turn, next turn +1 Strength. Upgrade: also +1 Dexterity.
/// </summary>
public class ActorsInstinctPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    private bool _shouldGrantBuffNextTurn;

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(Cards.JhinKeywords.Flourish),
    ];

    public override Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side != CombatSide.Player)
        {
            return Task.CompletedTask;
        }

        JhinMagazineState? state = JhinMagazineStateRegistry.TryGet(Owner.Player);
        _shouldGrantBuffNextTurn = state is not null && state.FlourishCountThisTurn == 0;

        return Task.CompletedTask;
    }

    public void OnTurnStart()
    {
        if (!_shouldGrantBuffNextTurn)
        {
            return;
        }

        _shouldGrantBuffNextTurn = false;
        Flash();
        _ = JhinCombatActionUtil.ApplyOrStackStrength(Owner, 1);

        if (Amount > 1)
        {
            _ = JhinCombatActionUtil.ApplyOrStackDexterity(Owner, 1);
        }
    }
}
