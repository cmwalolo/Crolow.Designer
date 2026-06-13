using Crolow.Designer.Abstractions;
using Crolow.Designer.Core.Document;
using Crolow.Designer.Runtime.Application;
using Crolow.Designer.Runtime.Application.Commands;
using Crolow.Designer.Runtime.Modules.DocumentModule.Commands.Requests;

namespace Crolow.Designer.Runtime.Modules.DocumentModule.Commands;


[CommandParameter(typeof(CreateDocumentCommand))]
public sealed class CreateDocumentCommandHandler
    : ICommandHandler<CreateDocumentCommand, DesignDocument>
{
    private readonly DesignerRuntime _runtime;

    public CreateDocumentCommandHandler(
        DesignerRuntime runtime)
    {
        _runtime = runtime;
    }

    public async Task<ICommandResult<DesignDocument>>
        ExecuteAsync(
            CreateDocumentCommand command)
    {
        var document = new DesignDocument
        {
            Name = "New document"
        };

        return new CommandResult<DesignDocument>
        {
            ResponseCode = 0,
            ResponseMessage = "Document created successfully",
            Result = document
        };
    }

}
