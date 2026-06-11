using Crolow.Designer.Runtime.Application.Commands.Requests;
using Crolow.Designer.Runtime.Application.Commands.Results;
using Crolow.Designer.Runtime.Application.Events;
using Crolow.Designer.Runtime.Commands;

namespace Crolow.Designer.Runtime.Application.Commands;

public sealed class ClearSelectionCommandHandler<T>
    : ICommandHandler<
        ClearSelectionCommand<T>,
        SelectionResult<T>>
{
    private readonly DesignerRuntime _runtime;

    public ClearSelectionCommandHandler(
        DesignerRuntime runtime)
    {
        _runtime = runtime;
    }

    public async Task<SelectionResult<T>>
        ExecuteAsync(
            ClearSelectionCommand<T> command)
    {
        command.Owner.Selection.Clear();

        var result =
            new SelectionResult<T>
            {
                Owner = command.Owner,
                Items = []
            };

        await _runtime.Events.PublishAsync(
            new SelectionChangedEvent<T>(
                command.Owner,
                result.Items));

        return result;
    }
}
