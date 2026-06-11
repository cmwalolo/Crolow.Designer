using Crolow.Designer.Core.Document;
using Crolow.Designer.Runtime.Application.Commands.Requests;
using Crolow.Designer.Runtime.Application.Events;
using Crolow.Designer.Runtime.Commands;

namespace Crolow.Designer.Runtime.Application.Commands;


public sealed class CreateDocumentCommandHandler
    : ICommandHandler<
        CreateDocumentCommand,
        DesignDocument>
{
    private readonly DesignerRuntime _runtime;

    public CreateDocumentCommandHandler(
        DesignerRuntime runtime)
    {
        _runtime = runtime;
    }

    public async Task<DesignDocument>
        ExecuteAsync(
            CreateDocumentCommand command)
    {
        var document =
            new DesignDocument
            {
                Name = command.Name
            };

        _runtime.Documents.Add(
            document);

        await _runtime.Events.PublishAsync(
            new DocumentCreatedEvent(
                document));

        return document;
    }
}
