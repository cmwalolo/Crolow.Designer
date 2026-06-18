using Crolow.Designer.Abstractions;
using Crolow.Designer.Runtime.Application.Events;
using System.Collections.Concurrent;

public sealed class EventBus
{
    private readonly ConcurrentDictionary<Type, List<EventBusSubscription>> _handlers = new();

    public void Subscribe<TEvent>(Guid targetId, Func<TEvent, Task> handler)
            where TEvent : IEvent
    {
        var list = _handlers.GetOrAdd(typeof(TEvent), _ => new List<EventBusSubscription>());
        list.Add(new EventBusSubscription(targetId, evt => handler((TEvent)evt)));
    }

    public async Task PublishAsync<TEvent>(Guid targetId, TEvent evt)
            where TEvent : IEvent
    {
        if (!_handlers.TryGetValue(typeof(TEvent), out var handlers))
            return;

        foreach (var subscription in handlers)
        {
            if (subscription.Id == targetId)
            {
                await subscription.Handler(evt);
            }
        }
    }
}