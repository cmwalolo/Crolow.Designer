using Crolow.Designer.Common.Constants;
using Crolow.Designer.Common.Extensions;
using Crolow.Designer.Common.Runtime;
using Crolow.Designer.Core.Extensions;
using Crolow.Designer.Runtime.Application;
using Crolow.Designer.Runtime.Application.Commands;
using Crolow.Designer.Runtime.Modules.DocumentModule.Commands.Documents.Events;
using Crolow.Designer.Runtime.Modules.DocumentModule.Commands.Documents.Requests;

namespace Crolow.Designer.Runtime.Modules.DocumentModule.Commands.Documents;


[CommandParameter(typeof(CreateDocumentCommand))]
public sealed class CreateDocumentCommandHandler
    : ICommandHandler<CreateDocumentCommand, DocumentSession>
{
    private readonly DesignerRuntime _runtime;

    public CreateDocumentCommandHandler(
        DesignerRuntime runtime)
    {
        _runtime = runtime;
    }

    public async Task<ICommandResult<DocumentSession>>
        ExecuteAsync(
            CreateDocumentCommand command)
    {

        command.Request.Pages.Add(
            new Core.Scene.Nodes.PageNode
            {
                ParentId = command.Request.Id,
                ParentNode = command.Request,
                Name = "Default Layer",
                Size = command.Request.Size
            }
            );

        command.Request.Pages.ApplyParents();

        var session = new DocumentSession(command.Initiator, _runtime, command.Request);
        command.Initiator.Documents.Add(session);

        await _runtime.Events.PublishAsync(GuidSources.Documents.GenerateGuid(), new DocumentEvent(_runtime.Documents, session));

        return new CommandResult<DocumentSession>
        {
            ResponseCode = 0,
            ResponseMessage = "Document opened successfully",
            Result = session
        };
    }

}
