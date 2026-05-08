#nullable enable

using BaseLib.Abstracts;
using BaseLib.Patches.Localization;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;

namespace jhin.Powers;

/// <summary>
/// 幕间休息 / Intermission — First non-shoot attack each turn: +3 dmg. Upgrade: +5 dmg.
/// Implemented as a temporary Strength buff that is consumed after the first non-shoot attack.
/// </summary>
public class IntermissionPower : CustomPowerModel, IAddDumbVariablesToPowerDescription
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    private bool _triggeredThisTurn;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [];

    public void AddDumbVariablesToPowerDescription(LocString description)
    {
        description.Add("bonusDamage", Amount > 1 ? 5 : 3);
    }

    public override Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        _triggeredThisTurn = false;
        return Task.CompletedTask;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (_triggeredThisTurn)
        {
            return;
        }

        if (cardPlay.Card.Owner != Owner.Player || cardPlay.Card.Type != CardType.Attack)
        {
            return;
        }

        if (cardPlay.Card is Cards.AbstractShootCard)
        {
            return;
        }

        _triggeredThisTurn = true;
        Flash();
        int bonusDamage = Amount > 1 ? 5 : 3;
        await Actions.JhinCombatActionUtil.ApplyOrStackStrength(Owner, bonusDamage);
    }
}
