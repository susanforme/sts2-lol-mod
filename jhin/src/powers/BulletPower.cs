#nullable enable

using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using jhin.Magazine;
using jhin.Relics;

namespace jhin.Powers;

public class BulletPower : CustomPowerModel
{
    private JhinMagazineState? _subscribedState;

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

    public void SubscribeToMagazine(JhinMagazineState state)
    {
        if (ReferenceEquals(_subscribedState, state))
        {
            SyncFrom(state);
            return;
        }

        UnsubscribeFromMagazine(_subscribedState);
        _subscribedState = state;
        state.StateChanged += OnMagazineStateChanged;
        SyncFrom(state);
    }

    public void UnsubscribeFromMagazine(JhinMagazineState? state)
    {
        if (state is null)
        {
            return;
        }

        state.StateChanged -= OnMagazineStateChanged;
        if (ReferenceEquals(_subscribedState, state))
        {
            _subscribedState = null;
        }
    }

    private void OnMagazineStateChanged(JhinMagazineState state, MagazineStateChange change)
    {
        if (change.HasFlag(MagazineStateChange.Bullets))
        {
            SyncFrom(state);
        }

        if (change.HasFlag(MagazineStateChange.MaxBullets)
            || change.HasFlag(MagazineStateChange.FlourishState)
            || change.HasFlag(MagazineStateChange.TurnState)
            || change.HasFlag(MagazineStateChange.PlayStats))
        {
            RefreshCombatState();
        }
    }

    private static void RefreshCombatState()
    {
        CombatManager? combatManager = CombatManager.Instance;
        if (combatManager?.StateTracker is null)
        {
            return;
        }

        var state = combatManager.DebugOnlyGetState();
        if (state is null)
        {
            return;
        }

        combatManager.StateTracker.SetState(state);
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

        state.IncrementCardsPlayed();

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

        if (Owner?.Player is not null)
        {
            Owner.Player.GetRelic<Relics.PerfectStage>()?.CheckTurnEnd();
        }

        return Task.CompletedTask;
    }
}
