using Crolow.Designer.Common.Runtime;
using Crolow.Designer.Core.Document;

namespace Crolow.Designer.Runtime.Modules.DocumentModule.Commands.Documents.Requests;

public sealed record OpenDocumentCommand : ICommandParameter<DocumentSessionManager, string, DesignDocument>
{
    public OpenDocumentCommand(Guid refId, DocumentSessionManager session, string documentFile)
    {
        ReferenceId = refId;
        Initiator = session;
        Request = documentFile;
    }
    public DocumentSessionManager Initiator { get; set; }
    public string Request { get; set; }
    public Guid ReferenceId { get; set; }
}
