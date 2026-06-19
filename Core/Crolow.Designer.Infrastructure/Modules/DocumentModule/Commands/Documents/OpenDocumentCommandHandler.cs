using Crolow.Designer.Common;
using Crolow.Designer.Common.Runtime;
using Crolow.Designer.Core.Document;
using Crolow.Designer.Runtime.Application;
using Crolow.Designer.Runtime.Application.Commands;
using Crolow.Designer.Runtime.Modules.DocumentModule.Commands.Documents.Requests;

namespace Crolow.Designer.Runtime.Modules.DocumentModule.Commands.Documents;


[CommandParameter(typeof(OpenDocumentCommand))]
public sealed class OpenDocumentCommandHandler
    : ICommandHandler<OpenDocumentCommand, DocumentSession>
{
    private readonly DesignerRuntime _runtime;

    public OpenDocumentCommandHandler(
        DesignerRuntime runtime)
    {
        _runtime = runtime;
    }

    public async Task<ICommandResult<DocumentSession>>
        ExecuteAsync(
            OpenDocumentCommand command)
    {
        string fileContent = System.IO.File.ReadAllText(command.Request);

        var document = System.Text.Json.JsonSerializer.Deserialize<DesignDocument>(
            fileContent,
            new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }
        );

        document.FilePath = command.Request;

        var session = new DocumentSession(_runtime.Documents, _runtime, document);

        command.Initiator.Documents.Add(session);


        return new CommandResult<DocumentSession>
        {
            ResponseCode = 0,
            ResponseMessage = "Document opened successfully",
            Result = session
        };
    }

}
