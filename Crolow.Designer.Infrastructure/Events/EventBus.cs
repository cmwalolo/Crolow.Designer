using System.Collections.Concurrent;

namespace Crolow.Designer.Runtime.Events;

public sealed class EventBus
{
    private readonly ConcurrentDictionary<
        Type,
        List<Func<object, Task>>> _handlers = new();

    public void Subscribe<TEvent>(
        Func<TEvent, Task> handler)
        where TEvent : IEvent
    {
        var list =
            _handlers.GetOrAdd(
                typeof(TEvent),
                _ => []);

        list.Add(
            evt => handler((TEvent)evt));
    }

    public async Task PublishAsync<TEvent>(
        TEvent evt)
        where TEvent : IEvent
    {
        if (!_handlers.TryGetValue(
                typeof(TEvent),
                out var handlers))
            return;

        foreach (var handler in handlers)
            await handler(evt);
    }
}
