namespace Crolow.Designer.Common.Runtime;

public interface ICommandHandler<TCommand, TResult>
{
    ICommandResult<TResult> Execute(TCommand command);
}
