using Crolow.Designer.Core.Document;
using Crolow.Designer.Runtime.Application.Commands;

namespace Crolow.Designer.Runtime.Modules.DocumentModule.Commands.Requests;

public sealed record CreateDocumentCommand : ICommandParameter<DocumentSessionManager, object, DesignDocument>
{
    public CreateDocumentCommand(DocumentSessionManager session)
    {
        Initiator = session;
    }
    public DocumentSessionManager Initiator { get; set; }
    public object Request { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
}
