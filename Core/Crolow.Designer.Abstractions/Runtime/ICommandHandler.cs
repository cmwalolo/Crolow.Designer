namespace Crolow.Designer.Common.Runtime;

public interface ICommandHandler<TCommand, TResult>
{
    Task<ICommandResult<TResult>> ExecuteAsync(TCommand command);
}
