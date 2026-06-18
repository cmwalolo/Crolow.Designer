namespace Crolow.Designer.Runtime.Application.Commands;

public interface ICommandParameter<TInitiator, TRequest, TResult>
{
    TInitiator Initiator { get; set; }
    TRequest Request { get; set; }
}

