using Crolow.Designer.Runtime.Application.Commands;
using Crolow.Designer.Runtime.Events;

namespace Crolow.Designer.Runtime.Application;
#region Commands

#endregion


public sealed class DesignerRuntime
{
    public DocumentSessionManager Documents { get; } = new();

    public EventBus Events { get; }
        = new();

    public CommandDispatcher Commands { get; }

    public DesignerRuntime()
    {
        Commands = new CommandDispatcher(this);
    }
}
