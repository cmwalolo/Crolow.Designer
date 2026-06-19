namespace Crolow.Designer.Common.Runtime;

public interface ICommandParameter<TInitiator, TRequest, TResult>
{
    TInitiator Initiator { get; set; }
    TRequest Request { get; set; }
}

