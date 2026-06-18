using Crolow.Designer.Core.Document;
using Crolow.Designer.Runtime.Application.Commands;

namespace Crolow.Designer.Runtime.Modules.DocumentModule.Commands.Documents.Requests;

public sealed record OpenDocumentCommand : ICommandParameter<DocumentSessionManager, string, DesignDocument>
{
    public OpenDocumentCommand(DocumentSessionManager session, string documentFile)
    {
        Initiator = session;
        Request = documentFile;
    }
    public DocumentSessionManager Initiator { get; set; }
    public string Request { get; set; }
}
