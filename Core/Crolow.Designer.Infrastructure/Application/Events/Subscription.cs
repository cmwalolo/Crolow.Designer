using Crolow.Designer.Abstractions;

namespace Crolow.Designer.Runtime.Application.Events
{
    public sealed class EventBusSubscription : IDisposable
    {
        private readonly EventBus _bus;
        internal Type EventType { get; }

        public Guid Id { get; }
        public Func<IEvent, Task> Handler { get; }

        internal EventBusSubscription(
            EventBus bus,
            Type eventType,
            Guid id,
            Func<IEvent, Task> handler)
        {
            _bus = bus;
            EventType = eventType;
            Id = id;
            Handler = handler;
        }

        public void Dispose()
        {
            _bus.Unsubscribe(this);
        }
    }
}

