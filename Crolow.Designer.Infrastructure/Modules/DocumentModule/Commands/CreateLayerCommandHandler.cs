using Crolow.Designer.Abstractions;
using Crolow.Designer.Core.Scene.Nodes;
using Crolow.Designer.Runtime.Application;
using Crolow.Designer.Runtime.Application.Commands;
using Crolow.Designer.Runtime.Modules.DocumentModule.Commands.Requests;

namespace Crolow.Designer.Runtime.Modules.DocumentModule.Commands;

[CommandParameter(typeof(CreateLayerCommand))]
public sealed class CreateLayerCommandHandler
    : ICommandHandler<CreateLayerCommand, LayerNode>
{
    private readonly DesignerRuntime _runtime;

    public CreateLayerCommandHandler(
        DesignerRuntime runtime)
    {
        _runtime = runtime;
    }

    public async Task<Abstractions.ICommandResult<LayerNode>> ExecuteAsync(CreateLayerCommand command)
    {
        var layer = new LayerNode { };
        command.Initiator.Layers.Add(layer);

        return new CommandResult<LayerNode>
        {
            ResponseCode = 0,
            ResponseMessage = "Layer created successfully",
            Result = layer
        };
    }
}
