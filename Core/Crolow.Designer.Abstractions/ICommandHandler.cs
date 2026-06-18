using Crolow.Designer.Abstractions;

namespace Crolow.Designer.Runtime.Application.Commands;

public interface ICommandHandler<TCommand, TResult>
{
    Task<ICommandResult<TResult>> ExecuteAsync(TCommand command);
}
