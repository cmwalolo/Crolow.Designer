using Crolow.Designer.Runtime.Application.Commands.Requests;
using Crolow.Designer.Runtime.Application.Commands.Results;
using Crolow.Designer.Runtime.Application.Events;
using Crolow.Designer.Runtime.Commands;

namespace Crolow.Designer.Runtime.Application.Commands;

public sealed class SelectCommandHandler<T>
    : ICommandHandler<
        SelectCommand<T>,
        SelectionResult<T>>
{
    private readonly DesignerRuntime _runtime;

    public SelectCommandHandler(
        DesignerRuntime runtime)
    {
        _runtime = runtime;
    }

    public async Task<SelectionResult<T>>
        ExecuteAsync(
            SelectCommand<T> command)
    {
        command.Owner.Selection.Select(
            command.Target);

        var result =
            new SelectionResult<T>
            {
                Owner = command.Owner,
                Items =
                    command.Owner.Selection.Items
                        .ToList()
            };

        await _runtime.Events.PublishAsync(
            new SelectionChangedEvent<T>(
                command.Owner,
                result.Items));

        return result;
    }
}
