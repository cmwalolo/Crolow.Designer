#region Demo

using Crolow.Designer.Runtime.Application;
using Crolow.Designer.Runtime.Modules.DocumentModule;
using Crolow.Designer.Runtime.Modules.DocumentModule.Commands.Requests;

public static class Program
{
    public static async Task Main()
    {
        var runtime = new DesignerRuntime();

        var documentSessionManager = runtime.Documents;
        var document =
             await runtime.Commands.ExecuteAsync(new CreateDocumentCommand(documentSessionManager));
        var documentSession = new DocumentSession { Document = document.Result };
        documentSessionManager.Documents.Add(documentSession);

        var layer = await runtime.Commands.ExecuteAsync(
                new CreateLayerCommand(document.Result));

        await runtime.Commands.ExecuteAsync(
            new CreateRectangleCommand(
                layer.Result));

        await runtime.Commands.ExecuteAsync(
            new CreateRectangleCommand(
                layer.Result));

        var layer2 = await runtime.Commands.ExecuteAsync(
                new CreateLayerCommand(
                    document.Result));

        await runtime.Commands.ExecuteAsync(
            new CreateRectangleCommand(
                layer2.Result));
        await runtime.Commands.ExecuteAsync(
            new CreateRectangleCommand(
                layer2.Result));


        var layers = document.Result.Layers.Count();
        var objects = document.Result.Layers.SelectMany(x => x.Children).Count();

        Console.WriteLine(
            $"# Layers : {layers}");
        Console.WriteLine(
            $"# Objects : {objects}");

    }
}
#endregion
