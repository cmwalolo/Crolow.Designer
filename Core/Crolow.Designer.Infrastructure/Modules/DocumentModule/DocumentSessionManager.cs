using Crolow.Designer.Core.Document;
using Crolow.Designer.Runtime.Application;
using Crolow.Designer.Runtime.Modules.DocumentModule.Commands.Documents.Requests;

namespace Crolow.Designer.Runtime.Modules.DocumentModule;

public sealed class DocumentSessionManager
{
    public DesignerRuntime Runtime { get; set; }
    public IList<DocumentSession> Documents { get; } = new List<DocumentSession>();

    public DocumentSessionManager(DesignerRuntime runtime)
    {
        this.Runtime = runtime;
    }

    public async Task<DocumentSession> CreateDocument(DesignDocument document)
    {
        var result =
             await Runtime.Commands.ExecuteAsync(new CreateDocumentCommand(this, document));

        if (result.ResponseCode == 0)
        {
            Documents.Add(result.Result);
            return result.Result;
        }
        else
        {
            // TODO: Handle error   
        }
        return default;
    }

    public async Task<DocumentSession> OpenDocument(string documentPath)
    {
        var result =
             await Runtime.Commands.ExecuteAsync(new OpenDocumentCommand(this, documentPath));

        if (result.ResponseCode == 0)
        {
            var documentSession = new DocumentSession(this, this.Runtime, result.Result);
            Documents.Add(documentSession);
            return documentSession;
        }
        else
        {
            // TODO: Handle error   
        }
        return default;

    }

    public void CloseDocument(DesignDocument document)
    {
    }
}