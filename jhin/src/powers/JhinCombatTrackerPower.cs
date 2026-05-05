#nullable enable

using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using jhin.Actions;
using jhin.Magazine;

namespace jhin.Powers;

/// <summary>
/// Hidden power applied at combat start to track card type usage per turn.
/// Used by cards like EncoreBullet to check if a Skill was played this turn.
/// </summary>
public class JhinCombatTrackerPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    public new bool IsVisible => false;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [];

    public override Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner.Player)
        {
            return Task.CompletedTask;
        }

        JhinMagazineState? state = JhinMagazineStateRegistry.TryGet(Owner.Player);
        if (state is null)
        {
            return Task.CompletedTask;
        }

        if (cardPlay.Card.Type == CardType.Skill)
        {
            state.RecordSkillPlayed();
        }

        if (cardPlay.Card.Type == CardType.Attack)
        {
            state.RecordAttackCardPlayed();

            if (cardPlay.Card is not Cards.AbstractShootCard)
            {
                state.RecordNonShootAttackPlayed();
            }
        }

        Owner.Player?.Creature?.GetPower<DeathIsArtPower>()?.AfterCardPlayed(cardPlay);

        return Task.CompletedTask;
    }
}
