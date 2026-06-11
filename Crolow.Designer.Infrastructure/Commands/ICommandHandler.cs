namespace Crolow.Designer.Runtime.Commands;

public interface ICommandHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    Task<TResult> ExecuteAsync(
        TCommand command);
}
