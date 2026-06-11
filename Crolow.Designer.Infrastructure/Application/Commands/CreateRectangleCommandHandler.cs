using Crolow.Designer.Core.Scene.Nodes.Objects;
using Crolow.Designer.Runtime.Application.Commands.Requests;
using Crolow.Designer.Runtime.Application.Events;
using Crolow.Designer.Runtime.Commands;

namespace Crolow.Designer.Runtime.Application.Commands;

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

    public async Task<RectangleShape>
        ExecuteAsync(
            CreateRectangleCommand command)
    {
        var rectangle =
            new RectangleShape
            {
                Name = command.Name
            };

        command.Layer.Children.Add(
            rectangle);

        await _runtime.Events.PublishAsync(
            new RectangleCreatedEvent(
                command.Layer,
                rectangle));

        return rectangle;
    }
}
