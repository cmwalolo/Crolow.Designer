using Crolow.Designer.Common.Constants;
using Crolow.Designer.Common.Event;
using Crolow.Designer.Common.Extensions;
using Crolow.Designer.Core.Scene.Nodes;

namespace Crolow.Designer.Runtime.Modules.DocumentModule.Commands.GroupNodes.Events;

public sealed record DocumentLayerEvent : IEvent<DocumentSession, PageNode>
{
    public DocumentLayerEvent(DocumentSession session, List<PageNode> documents)
    {
        ReferenceId = GuidSources.Documents.GenerateGuid();
        Source = session;
        Target = documents;
    }

    public DocumentLayerEvent(DocumentSession session, PageNode document)
    {
        ReferenceId = GuidSources.Documents.GenerateGuid();
        Source = session;
        Target = new List<PageNode> { document };
    }

    public Guid ReferenceId { get; }
    public EventTarget EventTarget { get; } = EventTarget.DocumentSessions;
    public EventAction EventAction { get; } = EventAction.ObjectCreated;
    public DocumentSession Source { get; }
    public List<PageNode> Target { get; }
}