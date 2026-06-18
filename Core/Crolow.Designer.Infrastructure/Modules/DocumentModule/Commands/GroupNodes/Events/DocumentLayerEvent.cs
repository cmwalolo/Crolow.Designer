using Crolow.Designer.Abstractions;
using Crolow.Designer.Core.Scene.Nodes;
using Crolow.Designer.Runtime.Modules.DocumentModule.Commands.Documents.Events;

namespace Crolow.Designer.Runtime.Modules.DocumentModule.Commands.GroupNodes.Events;

public sealed record DocumentLayerEvent : IEvent<DocumentSession, LayerNode>
{
    public DocumentLayerEvent(DocumentSession session, List<LayerNode> documents)
    {
        ReferenceId = GuidSources.Documents.GenerateGuid();
        Source = session;
        Target = documents;
    }

    public DocumentLayerEvent(DocumentSession session, LayerNode document)
    {
        ReferenceId = GuidSources.Documents.GenerateGuid();
        Source = session;
        Target = new List<LayerNode> { document };
    }

    public Guid ReferenceId { get; }
    public EventTarget EventTarget { get; } = EventTarget.DocumentSessions;
    public EventAction EventAction { get; } = EventAction.ObjectCreated;
    public DocumentSession Source { get; }
    public List<LayerNode> Target { get; }
}