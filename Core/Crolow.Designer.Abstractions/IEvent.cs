namespace Crolow.Designer.Abstractions;



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

public enum EventTarget
{
    Runtime = 1,
    DocumentSessions = 2,
    DocumentSession = 4
}

public enum EventAction
{
    ObjectCreated,
    ObjectUpdated,
    ObjectDeleted,
    ChildrenCreated,
    ChildrenUpdated,
    ChildrenDeleted
}