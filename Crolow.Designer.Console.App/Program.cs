#region Demo

using Crolow.Designer;
using Crolow.Designer.Core.Document;
using Crolow.Designer.Core.Scene.Nodes;
using Crolow.Designer.Runtime.Application.Commands;
using Crolow.Designer.Runtime.Application.Commands.Requests;
using Crolow.Designer.Runtime.Application.Events;

public static class Program
{
    public static async Task Main()
    {
        var runtime =
            new DesignerRuntime();

        runtime.Commands.Register(
            new CreateDocumentCommandHandler(runtime));

        runtime.Commands.Register(
            new CreateLayerCommandHandler(runtime));

        runtime.Commands.Register(
            new CreateRectangleCommandHandler(runtime));

        runtime.Commands.Register(
            new SelectCommandHandler<Layer>(runtime));

        runtime.Commands.Register(
            new SelectCommandHandler<SceneNode>(runtime));

        runtime.Commands.Register(
            new ClearSelectionCommandHandler<Layer>(runtime));

        runtime.Commands.Register(
            new ClearSelectionCommandHandler<SceneNode>(runtime));

        runtime.Events.Subscribe<
                LayerCreatedEvent>(
                evt =>
                {
                    Console.WriteLine(
                        $"Layer Created => {evt.Layer.Name}");

                    return Task.CompletedTask;
                });

        var document =
            await runtime.Commands.ExecuteAsync(
                new CreateDocumentCommand(
                    "Landing Page"));

        var layer =
            await runtime.Commands.ExecuteAsync(
                new CreateLayerCommand(
                    document,
                    "Desktop"));

        var hero =
            await runtime.Commands.ExecuteAsync(
                new CreateRectangleCommand(
                    layer,
                    "Hero Banner"));

        var selectedLayers =
            await runtime.Commands.ExecuteAsync(
                new SelectCommand<Layer>(
                    document,
                    layer));

        var selectedObjects =
            await runtime.Commands.ExecuteAsync(
                new SelectCommand<SceneNode>(
                    layer,
                    hero));

        Console.WriteLine(
            $"Selected Layers : {selectedLayers.Items.Count}");

        Console.WriteLine(
            $"Selected Objects : {selectedObjects.Items.Count}");
    }
}
#endregion
