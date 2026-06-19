using Crolow.Designer.Common.Runtime;

namespace Crolow.Designer.Runtime.Application.Commands;

public class CommandResult<TResult> : ICommandResult<TResult>
{
    public int ResponseCode { get; set; }
    public string ResponseMessage { get; set; }
    public TResult Result { get; set; } = default;
}
