#nullable enable

using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Rooms;

namespace jhin.Relics;

public abstract class AbstractJhinRelic : CustomRelicModel
{
    private bool _eventsSubscribed;

    public sealed override Task BeforeCombatStart()
    {
        SubscribeEvents();
        return OnBeforeCombatStart();
    }

    public sealed override Task AfterCombatEnd(CombatRoom room)
    {
        UnsubscribeEvents();
        return OnAfterCombatEnd(room);
    }

    protected virtual Task OnBeforeCombatStart() => Task.CompletedTask;

    protected virtual Task OnAfterCombatEnd(CombatRoom room) => Task.CompletedTask;

    protected void SubscribeEvents()
    {
        if (_eventsSubscribed)
        {
            return;
        }

        SubscribeEventHandlers();
        _eventsSubscribed = true;
    }

    protected void UnsubscribeEvents()
    {
        if (!_eventsSubscribed)
        {
            return;
        }

        UnsubscribeEventHandlers();
        _eventsSubscribed = false;
    }

    protected virtual void SubscribeEventHandlers()
    {
    }

    protected virtual void UnsubscribeEventHandlers()
    {
    }
}
