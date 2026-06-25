using Crolow.Designer.Common.Constants;
using Crolow.Designer.Common.Extensions;
using Crolow.Designer.Common.Runtime;
using Crolow.Designer.Core.Extensions;
using Crolow.Designer.Core.Scene.Nodes;
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

        var p = new PageNode
        {
            ParentId = command.Request.Id,
            ParentNode = command.Request,
            Name = "Default Page",
            Size = command.Request.Size
        };

        command.Request.Pages.Add(p);
        p.Children.Add(new LayerNode
        {
            ParentId = command.Request.Id,
            ParentNode = command.Request,
            Name = "Default Layer",
            Size = command.Request.Size
        });

        command.Request.ApplyParents();

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
