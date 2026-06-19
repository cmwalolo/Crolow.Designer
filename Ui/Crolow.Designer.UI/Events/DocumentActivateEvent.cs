using Crolow.Designer.Common.Constants;
using Crolow.Designer.Common.Event;
using Crolow.Designer.Common.Extensions;
using Crolow.Designer.UI;

namespace Crolow.Designer.Runtime.Modules.DocumentModule.Commands.Documents.Events;

public sealed record DocumentActivateEvent : IEvent<DocumentsController, DocumentController>
{

    public DocumentActivateEvent(DocumentsController session, List<DocumentController> documents)
    {
        ReferenceId = GuidSources.Documents.GenerateGuid();
        Source = session;
        Target = documents;
    }

    public DocumentActivateEvent(DocumentsController session, DocumentController document)
    {
        ReferenceId = GuidSources.Documents.GenerateGuid();
        Source = session;
        Target = new List<DocumentController> { document };
    }

    public Guid ReferenceId { get; }
    public EventTarget EventTarget { get; } = EventTarget.DocumentSessions;
    public EventAction EventAction { get; } = EventAction.ObjectActivated;
    public DocumentsController Source { get; }
    public List<DocumentController> Target { get; }
}
