using Crolow.Designer.Common.Constants;
using Crolow.Designer.Common.Event;
using Crolow.Designer.Common.Extensions;

namespace Crolow.Designer.Runtime.Modules.DocumentModule.Commands.Documents.Events;

public sealed record DocumentEvent : IEvent<DocumentSessionManager, DocumentSession>
{
    DocumentSessionManager mm;
    DocumentSession dd;

    public DocumentEvent(DocumentSessionManager session, List<DocumentSession> documents)
    {
        ReferenceId = GuidSources.Documents.GenerateGuid();
        Source = session;
        Target = documents;
    }

    public DocumentEvent(DocumentSessionManager session, DocumentSession document)
    {
        ReferenceId = GuidSources.Documents.GenerateGuid();
        Source = session;
        Target = new List<DocumentSession> { document };
    }

    public Guid ReferenceId { get; }
    public EventTarget EventTarget { get; } = EventTarget.DocumentSessions;
    public EventAction EventAction { get; } = EventAction.ObjectCreated;
    public DocumentSessionManager Source { get; }
    public List<DocumentSession> Target { get; }
}
