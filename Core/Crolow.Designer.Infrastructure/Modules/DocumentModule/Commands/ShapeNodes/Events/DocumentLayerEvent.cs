using Crolow.Designer.Common.Constants;
using Crolow.Designer.Common.Event;
using Crolow.Designer.Common.Extensions;
using Crolow.Designer.Core.Scene.Nodes;

namespace Crolow.Designer.Runtime.Modules.DocumentModule.Commands.GroupNodes.Events;

public sealed record SceneNodeEvent : IEvent<SceneNode, SceneNode>
{
    public SceneNodeEvent(SceneNode source, List<SceneNode> target)
    {
        ReferenceId = GuidSources.Documents.GenerateGuid();
        Source = source;
        Target = target;
    }

    public SceneNodeEvent(SceneNode source, SceneNode target)
    {
        ReferenceId = GuidSources.Documents.GenerateGuid();
        Source = source;
        Target = new List<SceneNode> { target };
    }

    public Guid ReferenceId { get; }
    public EventTarget EventTarget { get; } = EventTarget.DocumentSessions;
    public EventAction EventAction { get; } = EventAction.ObjectCreated;
    public SceneNode Source { get; }
    public List<SceneNode> Target { get; }
}