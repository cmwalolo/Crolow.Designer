using Crolow.Designer.Runtime.Application.Commands;
using Crolow.Designer.Runtime.Modules.DocumentModule;

namespace Crolow.Designer.Runtime.Application;

public sealed class DesignerRuntime
{
    public DocumentSessionManager Documents;

    public EventBus Events { get; }
        = new();

    public CommandDispatcher Commands { get; }

    public DesignerRuntime()
    {
        Documents = new DocumentSessionManager(this);
        Commands = new CommandDispatcher(this);
    }
}
