#nullable enable

using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using jhin.Actions;

namespace jhin.Powers;

public class FlourishTempoPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public override LocString Description => new("powers", Amount > 1 ? "JHIN-FLOURISH_TEMPO_POWER_PLUS.description" : "JHIN-FLOURISH_TEMPO_POWER.description");

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(Cards.JhinKeywords.Flourish),
    ];

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner.Player || cardPlay.Card is not Cards.AbstractShootCard || !FlourishContext.IsActive)
        {
            return;
        }

        Flash();
        await PlayerCmd.GainEnergy(1m, Owner.Player);

        if (Amount > 1)
        {
            await JhinCombatActionUtil.Draw(context, Owner.Player, 1);
        }
    }
}
