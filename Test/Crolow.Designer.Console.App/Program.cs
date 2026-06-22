#region Demo

using Crolow.Designer.Core.Scene.Nodes;
using Crolow.Designer.Core.Scene.Nodes.Objects;
using Crolow.Designer.Runtime.Application;
using Crolow.Designer.Runtime.Modules.DocumentModule.Commands.Documents.Requests;
using Crolow.Designer.Runtime.Modules.DocumentModule.Commands.Layers.Requests;
using Crolow.Designer.Runtime.Modules.DocumentModule.Commands.Shapes.Requests;

public static class Program
{
    public static async Task Main()
    {
        var runtime = new DesignerRuntime();

        var documentSessionManager = runtime.Documents;

        var document =
             await runtime.Commands.ExecuteAsync(new CreateDocumentCommand(documentSessionManager, new Crolow.Designer.Core.Document.DesignDocument()));

        var layer = await runtime.Commands.ExecuteAsync(
                new CreateLayerCommand(document.Result.Document, new PageNode { Name = "Layer 1" }));

        await runtime.Commands.ExecuteAsync(
            new CreateSceneNodeCommand(
                layer.Result, new RectangleShape { Name = "Rectangle 1" }));

        await runtime.Commands.ExecuteAsync(
            new CreateSceneNodeCommand(
                layer.Result, new RectangleShape { Name = "Rectangle 2" }));

        var layer2 = await runtime.Commands.ExecuteAsync(
                new CreateLayerCommand(document.Result.Document, new PageNode { Name = "Layer 2" }));

        await runtime.Commands.ExecuteAsync(
            new CreateSceneNodeCommand(
                layer2.Result, new RectangleShape { Name = "Rectangle 1" }));

        await runtime.Commands.ExecuteAsync(
            new CreateSceneNodeCommand(
                layer2.Result, new RectangleShape { Name = "Rectangle 2" }));


        var layers = document.Result.Document.Pages.Count();
        var objects = document.Result.Document.Pages.SelectMany(x => x.Children).Count();

        Console.WriteLine(
            $"# Layers : {layers}");
        Console.WriteLine(
            $"# Objects : {objects}");

    }
}
#endregion
