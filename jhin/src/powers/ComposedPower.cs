#nullable enable

using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using jhin.Actions;
using jhin.Magazine;

namespace jhin.Powers;

/// <summary>
/// 从容不迫 / Composed — First flourish each combat: draw 2.
/// Subscribes to FlourishEventBus. Subscription managed by MagazineHooks at combat start/end.
/// </summary>
public class ComposedPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(Cards.JhinKeywords.Flourish),
    ];

    public void SubscribeEvents()
    {
        FlourishEventBus.OnFlourishTriggered += OnFlourishTriggered;
    }

    private void OnFlourishTriggered(PlayerChoiceContext choiceContext, Player player, JhinMagazineState state)
    {
        if (player != Owner?.Player)
        {
            return;
        }

        Flash();
        _ = JhinCombatActionUtil.Draw(choiceContext, Owner.Player, 2);
    }
}
