using Crolow.Designer.Common.Constants;
using Crolow.Designer.Common.Extensions;
using Crolow.Designer.Common.Runtime;
using Crolow.Designer.Core.Scene.Nodes;
using Crolow.Designer.Runtime.Application;
using Crolow.Designer.Runtime.Application.Commands;
using Crolow.Designer.Runtime.Modules.DocumentModule.Commands.GroupNodes.Events;
using Crolow.Designer.Runtime.Modules.DocumentModule.Commands.Shapes.Requests;

namespace Crolow.Designer.Runtime.Modules.DocumentModule.Commands;

[CommandParameter(typeof(CreateSceneNodeCommand))]
public sealed class CreateSceneNodeCommandHandler : ICommandHandler<CreateSceneNodeCommand, SceneNode>
{

    public DesignerRuntime _runtime;
    public CreateSceneNodeCommandHandler(DesignerRuntime runtime)
    {
        _runtime = runtime;
    }

    public ICommandResult<SceneNode> Execute(CreateSceneNodeCommand command)
    {
        command.Initiator.Children.Add(command.Request);
        command.Request.ParentId = command.Initiator.Id;
        command.Request.ParentNode = command.Initiator;
        command.Request.Position = command.Initiator.Children.Max(p => p.Position) + 1;

        _runtime.Events.PublishAsync(GuidSources.Document.GenerateGuid(), new SceneNodeEvent(command.Initiator, command.Request));

        return new CommandResult<SceneNode>
        {
            ResponseCode = 0,
            ResponseMessage = "Shape created successfully",
            Result = command.Request
        };
    }
}