using Crolow.Designer.Common.Event;
using Crolow.Designer.Runtime.Application.Events;
using System.Collections.Concurrent;

public sealed class EventBus
{
    private readonly ConcurrentDictionary<Type, List<EventBusSubscription>> _handlers = new();

    public IDisposable Subscribe<TEvent>(
        Guid targetId,
        Func<TEvent, Task> handler)
        where TEvent : IEvent
    {
        var list = _handlers.GetOrAdd(typeof(TEvent), _ => new List<EventBusSubscription>());

        var subscription = new EventBusSubscription(
            this,
            typeof(TEvent),
            targetId,
            evt => handler((TEvent)evt));

        lock (list)
        {
            list.Add(subscription);
        }

        return subscription;
    }

    internal void Unsubscribe(EventBusSubscription subscription)
    {
        if (_handlers.TryGetValue(subscription.EventType, out var list))
        {
            lock (list)
            {
                list.Remove(subscription);
            }
        }
    }

    public async Task PublishAsync<TEvent>(Guid targetId, TEvent evt)
        where TEvent : IEvent
    {
        if (!_handlers.TryGetValue(typeof(TEvent), out var handlers))
            return;

        EventBusSubscription[] subscriptions;

        lock (handlers)
        {
            subscriptions = handlers.ToArray();
        }

        foreach (var subscription in subscriptions)
        {
            if (subscription.Id == targetId)
            {
                await subscription.Handler(evt);
            }
        }
    }
}