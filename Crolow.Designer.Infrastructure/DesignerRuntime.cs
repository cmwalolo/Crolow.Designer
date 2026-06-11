using Crolow.Designer.Core.Document;
using Crolow.Designer.Runtime.Commands;
using Crolow.Designer.Runtime.Events;

namespace Crolow.Designer;
#region Commands

#endregion


public sealed class DesignerRuntime
{
    public List<DesignDocument> Documents { get; }
        = [];

    public EventBus Events { get; }
        = new();

    public CommandDispatcher Commands { get; }

    public DesignerRuntime()
    {
        Commands = new CommandDispatcher(this);
    }
}
