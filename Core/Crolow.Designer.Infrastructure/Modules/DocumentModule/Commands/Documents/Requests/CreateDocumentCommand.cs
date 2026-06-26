using Crolow.Designer.Common.Runtime;
using Crolow.Designer.Core.Document;

namespace Crolow.Designer.Runtime.Modules.DocumentModule.Commands.Documents.Requests;

public sealed record CreateDocumentCommand : ICommandParameter<DocumentSessionManager, DesignDocument, DocumentSession>
{
    public CreateDocumentCommand(Guid refId, DocumentSessionManager session, DesignDocument document)
    {
        ReferenceId = refId;
        Initiator = session;
        Request = document;
    }
    public DocumentSessionManager Initiator { get; set; }
    public DesignDocument Request { get; set; }

    public Guid ReferenceId { get; set; }
}
