using Crolow.Designer.Abstractions;
using Crolow.Designer.Core.Scene.Nodes;
using Crolow.Designer.Runtime.Application;
using Crolow.Designer.Runtime.Application.Commands;
using Crolow.Designer.Runtime.Modules.DocumentModule.Commands.Shapes.Requests;

namespace Crolow.Designer.Runtime.Modules.DocumentModule.Commands;

[CommandParameter(typeof(CreateSceneNodeCommand))]
public sealed class CreateSceneNodeCommandHandler
    : ICommandHandler<CreateSceneNodeCommand, SceneNode>
{
    private readonly DesignerRuntime _runtime;

    public CreateSceneNodeCommandHandler(
        DesignerRuntime runtime)
    {
        _runtime = runtime;
    }

    public async Task<ICommandResult<SceneNode>>
        ExecuteAsync(
            CreateSceneNodeCommand command)
    {
        command.Initiator.Children.Add(command.Request);
        command.Request.ParentId = command.Initiator.Id;
        command.Request.ParentNode = command.Initiator;

        return new CommandResult<SceneNode>
        {
            ResponseCode = 0,
            ResponseMessage = "Shapre created successfully",
            Result = command.Request
        };
    }
}