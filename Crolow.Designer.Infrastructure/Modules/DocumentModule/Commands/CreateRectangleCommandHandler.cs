using Crolow.Designer.Abstractions;
using Crolow.Designer.Core.Scene.Nodes.Objects;
using Crolow.Designer.Runtime.Application;
using Crolow.Designer.Runtime.Application.Commands;
using Crolow.Designer.Runtime.Modules.DocumentModule.Commands.Events;
using Crolow.Designer.Runtime.Modules.DocumentModule.Commands.Requests;

namespace Crolow.Designer.Runtime.Modules.DocumentModule.Commands;

[CommandParameter(typeof(CreateRectangleCommand))]
public sealed class CreateRectangleCommandHandler
    : ICommandHandler<
        CreateRectangleCommand,
        RectangleShape>
{
    private readonly DesignerRuntime _runtime;

    public CreateRectangleCommandHandler(
        DesignerRuntime runtime)
    {
        _runtime = runtime;
    }

    public async Task<ICommandResult<RectangleShape>>
        ExecuteAsync(
            CreateRectangleCommand command)
    {
        var rectangle = new RectangleShape
        {
            Name = "new Rectangle"
        };

        command.Initiator.Children.Add(rectangle);

        await _runtime.Events.PublishAsync(
            new RectangleCreatedEvent(
                command.Initiator,
                rectangle));

        return new CommandResult<RectangleShape>
        {
            ResponseCode = 0,
            ResponseMessage = "Rectangle created successfully",
            Result = rectangle
        };
    }
}