using Crolow.Designer.Core.Document;
using Crolow.Designer.Runtime.Application.Commands;

namespace Crolow.Designer.Runtime.Modules.DocumentModule.Commands.Documents.Requests;

public sealed record CreateDocumentCommand : ICommandParameter<DocumentSessionManager, DesignDocument, DocumentSession>
{
    public CreateDocumentCommand(DocumentSessionManager session, DesignDocument document)
    {
        Initiator = session;
        Request = document;
    }
    public DocumentSessionManager Initiator { get; set; }
    public DesignDocument Request { get; set; }
}
