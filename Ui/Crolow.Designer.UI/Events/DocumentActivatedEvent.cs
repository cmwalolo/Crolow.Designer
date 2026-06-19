using Crolow.Designer.Common.Constants;
using Crolow.Designer.Common.Event;
using Crolow.Designer.Common.Extensions;
using Crolow.Designer.UI;

namespace Crolow.Designer.Runtime.Modules.DocumentModule.Commands.Documents.Events;

public sealed record DocumentActivatedEvent : IEvent<DocumentsController, DocumentController>
{
    public DocumentActivatedEvent(DocumentsController session, bool newlyCreated, List<DocumentController> documents)
    {
        ReferenceId = GuidSources.Documents.GenerateGuid();
        Source = session;
        Target = documents;
        NewlyCreated = newlyCreated;
    }

    public DocumentActivatedEvent(DocumentsController session, bool newlyCreated, DocumentController document)
    {
        ReferenceId = GuidSources.Documents.GenerateGuid();
        Source = session;
        Target = new List<DocumentController> { document };
        NewlyCreated = newlyCreated;
    }

    public Guid ReferenceId { get; }
    public EventTarget EventTarget { get; } = EventTarget.DocumentSessions;
    public EventAction EventAction { get; } = EventAction.ObjectActivated;
    public DocumentsController Source { get; }
    public List<DocumentController> Target { get; }
    public bool NewlyCreated { get; set; }
}
