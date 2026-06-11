using System.Reflection;

namespace Crolow.Designer.Runtime.Commands;

public sealed class CommandDispatcher
{
    private readonly Dictionary<Type, object>
        _handlers = [];
    private readonly DesignerRuntime _runtime;

    public CommandDispatcher(DesignerRuntime runtime)
    {
        /// Todo some types need a generic Type ... so it can not be registered automatically. 

        /*_runtime = runtime;
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in assemblies)
        {
            RegisterAssembly(
                assembly);
        }*/
    }

    private void RegisterAssembly(
        Assembly assembly)
    {
        foreach (var type in assembly.GetTypes())
        {
            if (type.IsAbstract)
                continue;

            foreach (var iface in type.GetInterfaces())
            {
                if (!iface.IsGenericType)
                    continue;

                if (iface.GetGenericTypeDefinition()
                    != typeof(ICommandHandler<,>))
                    continue;

                Console.Write(type.Name);

                var handler =
                    Activator.CreateInstance(
                        type, new[] { _runtime });

                _handlers.Add(
                    iface.GetGenericArguments()[0],
                    handler!);
            }
        }
    }

    public void Register<TCommand, TResult>(
        ICommandHandler<TCommand, TResult> handler)
        where TCommand : ICommand<TResult>
    {
        _handlers[typeof(TCommand)] = handler;
    }

    public Task<TResult> ExecuteAsync<TResult>(
        ICommand<TResult> command)
    {
        dynamic handler =
            _handlers[command.GetType()];

        return handler.ExecuteAsync(
            (dynamic)command);
    }
}
