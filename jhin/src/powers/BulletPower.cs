using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using jhin.Magazine;

namespace jhin.Powers;

public class BulletPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override int DisplayAmount => Amount;

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(Cards.JhinKeywords.Bullet),
    ];

    public void SyncFrom(JhinMagazineState state)
    {
        SetAmount(state.Bullets);
    }

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

        DeathIsArtPower.CheckAfterCardPlayed(cardPlay, Owner);

        return Task.CompletedTask;
    }

    public override Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        LotusTrapPower.ApplyPendingWeak(side);
        return Task.CompletedTask;
    }
}
