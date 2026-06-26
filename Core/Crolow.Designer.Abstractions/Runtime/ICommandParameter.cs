namespace Crolow.Designer.Common.Runtime;

public interface ICommandParameter<TInitiator, TRequest, TResult>
{
    Guid ReferenceId { get; set; }
    TInitiator Initiator { get; set; }
    TRequest Request { get; set; }
}

