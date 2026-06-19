using Crolow.Designer.Common.Constants;

namespace Crolow.Designer.Common.Event;



public interface IEvent
{
    public Guid ReferenceId { get; }
    public EventTarget EventTarget { get; }
    public EventAction EventAction { get; }
}

public interface IEvent<TSource, TTarget> : IEvent
{
    public TSource Source { get; }
    public List<TTarget> Target { get; }
}
