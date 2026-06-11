using Crolow.Designer.Core.Document;
using Crolow.Designer.Runtime.Application.Commands.Requests;
using Crolow.Designer.Runtime.Application.Events;
using Crolow.Designer.Runtime.Commands;

namespace Crolow.Designer.Runtime.Application.Commands;

public sealed class CreateLayerCommandHandler
    : ICommandHandler<
        CreateLayerCommand,
        Layer>
{
    private readonly DesignerRuntime _runtime;

    public CreateLayerCommandHandler(
        DesignerRuntime runtime)
    {
        _runtime = runtime;
    }

    public async Task<Layer>
        ExecuteAsync(
            CreateLayerCommand command)
    {
        var layer =
            new Layer
            {
                Name = command.Name
            };

        command.Document.Layers.Add(
            layer);

        await _runtime.Events.PublishAsync(
            new LayerCreatedEvent(
                command.Document,
                layer));

        return layer;
    }
}
