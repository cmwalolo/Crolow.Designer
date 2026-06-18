namespace Crolow.Designer.Runtime.Application.Events
{
    public sealed class EventBusSubscription
    {
        public Guid Id { get; }
        public Func<object, Task> Handler { get; }

        public EventBusSubscription(Guid id, Func<object, Task> handler)
        {
            Id = id;
            Handler = handler;
        }
    }
}