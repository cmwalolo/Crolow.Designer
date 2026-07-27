using Crolow.Designer.Runtime.Application.Commands;
using Crolow.Designer.Runtime.Modules.DocumentModule;
using System.Reflection;

namespace Crolow.Designer.Runtime.Application;

public sealed class DesignerRuntime
{
    public DocumentSessionManager Documents;

    public EventBus Events { get; } = new();

    public CommandDispatcher Commands { get; }
    public ProvidersRegistration Providers { get; }

    public DesignerRuntime()
    {
        var assembly = Assembly.Load("Crolow.Designer.Graphics.Core");

        Documents = new DocumentSessionManager(this);
        Commands = new CommandDispatcher(this);
        Providers = new ProvidersRegistration(this);
    }
}
