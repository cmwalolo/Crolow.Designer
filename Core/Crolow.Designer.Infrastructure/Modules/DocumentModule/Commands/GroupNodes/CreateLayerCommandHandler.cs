using Crolow.Designer.Common.Runtime;
using Crolow.Designer.Core.Scene.Nodes;
using Crolow.Designer.Runtime.Application;
using Crolow.Designer.Runtime.Application.Commands;
using Crolow.Designer.Runtime.Modules.DocumentModule.Commands.Layers.Requests;

namespace Crolow.Designer.Runtime.Modules.DocumentModule.Commands.Layers;

[CommandParameter(typeof(CreateLayerCommand))]
public sealed class CreateLayerCommandHandler : ICommandHandler<CreateLayerCommand, PageNode>
{
    private readonly DesignerRuntime _runtime;

    public CreateLayerCommandHandler(DesignerRuntime runtime)
    {
        _runtime = runtime;
    }

    public ICommandResult<PageNode> Execute(CreateLayerCommand command)
    {
        var layer = new PageNode { };
        command.Initiator.Pages.Add(layer);
        layer.ParentId = command.Initiator.Id;
        layer.ParentNode = command.Initiator;

        return new CommandResult<PageNode>
        {
            ResponseCode = 0,
            ResponseMessage = "Layer created successfully",
            Result = layer
        };
    }
}
