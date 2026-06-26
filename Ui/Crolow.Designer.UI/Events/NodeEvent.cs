using Crolow.Designer.Common.Constants;
using Crolow.Designer.Common.Event;
using Crolow.Designer.Common.Extensions;
using Crolow.Designer.Core.Scene.Nodes;
using Crolow.Designer.UI;

namespace Crolow.Designer.Runtime.Modules.DocumentModule.Commands.Documents.Events;

public sealed record NodeEvent : IEvent<DocumentController, SceneNode>
{
    public NodeEvent(EventAction action, DocumentController session, bool newlyCreated, List<SceneNode> nodes)
    {
        ReferenceId = session.Session.Document.Id;
        Source = session;
        Target = nodes;
        NewlyCreated = newlyCreated;
        EventAction = action;
    }

    public NodeEvent(EventAction action, DocumentController session, bool newlyCreated, SceneNode node)
    {
        ReferenceId = GuidSources.Documents.GenerateGuid();
        Source = session;
        Target = new List<SceneNode> { node };
        NewlyCreated = newlyCreated;
        EventAction = action;
    }

    public Guid ReferenceId { get; }
    public EventTarget EventTarget { get; } = EventTarget.DocumentSessions;
    public EventAction EventAction { get; } = EventAction.ObjectActivated;
    public DocumentController Source { get; }
    public List<SceneNode> Target { get; }
    public bool NewlyCreated { get; set; }
}
