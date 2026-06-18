using Crolow.Designer.Abstractions;
using System.Reflection;

namespace Crolow.Designer.Runtime.Application.Commands;

public sealed class CommandDispatcher
{
    private readonly Dictionary<Type, object>
        _handlers = [];
    private readonly DesignerRuntime _runtime;

    public CommandDispatcher(DesignerRuntime runtime)
    {
        _runtime = runtime;
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in assemblies)
        {
            RegisterAssembly(assembly);
        }
    }

    public Task<ICommandResult<TResult>> ExecuteAsync<TCommand, TParameter, TResult>(
        ICommandParameter<TCommand, TParameter, TResult> command)
    {
        dynamic handler =
            _handlers[command.GetType()];

        return handler.ExecuteAsync((dynamic)command);
    }

    private void RegisterAssembly(Assembly assembly)
    {
        foreach (var type in assembly.GetTypes())
        {
            if (type.IsAbstract)
                continue;

            foreach (var iface in type.GetInterfaces())
            {
                if (!iface.IsGenericType)
                    continue;

                if (iface.GetGenericTypeDefinition() != typeof(ICommandHandler<,>))
                    continue;

                Console.Write(type.Name);

                var handler =
                    Activator.CreateInstance(
                        type, new[] { _runtime });

                var att = type.GetCustomAttributes<CommandParameterAttribute>()
                    .FirstOrDefault() is CommandParameterAttribute attribute
                    ? attribute.Type : null;

                if (att != null)
                {
                    _handlers.Add(att, handler!);
                }
            }
        }
    }
}
