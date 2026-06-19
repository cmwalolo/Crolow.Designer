using Crolow.Designer.Common.Runtime;
using Crolow.Designer.Core.Scene.Nodes;
using Crolow.Designer.Runtime.Application;
using Crolow.Designer.Runtime.Application.Commands;
using Crolow.Designer.Runtime.Modules.DocumentModule.Commands.Layers.Requests;

namespace Crolow.Designer.Runtime.Modules.DocumentModule.Commands.Layers;

[CommandParameter(typeof(CreateGroupNodeCommand))]
public sealed class CreateGroupNodeCommandHandler
    : ICommandHandler<CreateGroupNodeCommand, GroupNode>
{
    private readonly DesignerRuntime _runtime;

    public CreateGroupNodeCommandHandler(
        DesignerRuntime runtime)
    {
        _runtime = runtime;
    }

    public async Task<ICommandResult<GroupNode>> ExecuteAsync(CreateGroupNodeCommand command)
    {
        var node = new GroupNode { };
        command.Initiator.Children.Add(node);
        node.ParentId = command.Initiator.Id;
        node.ParentNode = command.Initiator;

        return new CommandResult<GroupNode>
        {
            ResponseCode = 0,
            ResponseMessage = "Group node created successfully",
            Result = node
        };
    }
}
