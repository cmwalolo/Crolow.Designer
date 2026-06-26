using Crolow.Designer.Common.Constants;
using Crolow.Designer.Common.Event;
using Crolow.Designer.Core.Scene.Nodes;

namespace Crolow.Designer.Runtime.Modules.DocumentModule.Commands.GroupNodes.Events;

public sealed record GroupNodeEvent : IEvent<GroupNode, SceneNode>
{
    public GroupNodeEvent(Guid refId, GroupNode parent, List<SceneNode> node)
    {
        ReferenceId = refId;
        Source = parent;
        Target = node;
    }

    public GroupNodeEvent(Guid refId, GroupNode parent, SceneNode document)
    {
        ReferenceId = refId;
        Source = parent;
        Target = new List<SceneNode> { document };
    }

    public Guid ReferenceId { get; }
    public EventTarget EventTarget { get; } = EventTarget.DocumentSessions;
    public EventAction EventAction { get; } = EventAction.ObjectCreated;
    public GroupNode Source { get; }
    public List<SceneNode> Target { get; }
}