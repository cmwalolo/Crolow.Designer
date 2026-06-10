using Crolow.Designer.Abstractions;
using Crolow.Designer.Core.Document;
using Crolow.Designer.Core.Scene.Nodes;
using Crolow.Designer.Core.Scene.Nodes.Objects;
using System.Collections.Concurrent;
using System.Reflection;

namespace Crolow.Designer;


#region Events

public interface IEvent
{
}

public sealed record DocumentCreatedEvent(
    DesignDocument Document)
    : IEvent;

public sealed record LayerCreatedEvent(
    DesignDocument Document,
    Layer Layer)
    : IEvent;

public sealed record RectangleCreatedEvent(
    Layer Layer,
    RectangleShape Rectangle)
    : IEvent;

public sealed record SelectionChangedEvent<T>(
    ISelectionOwner<T> Owner,
    IReadOnlyCollection<T> Items)
    : IEvent;

public sealed class EventBus
{
    private readonly ConcurrentDictionary<
        Type,
        List<Func<object, Task>>> _handlers = new();

    public void Subscribe<TEvent>(
        Func<TEvent, Task> handler)
        where TEvent : IEvent
    {
        var list =
            _handlers.GetOrAdd(
                typeof(TEvent),
                _ => []);

        list.Add(
            evt => handler((TEvent)evt));
    }

    public async Task PublishAsync<TEvent>(
        TEvent evt)
        where TEvent : IEvent
    {
        if (!_handlers.TryGetValue(
                typeof(TEvent),
                out var handlers))
            return;

        foreach (var handler in handlers)
            await handler(evt);
    }
}

#endregion

#region Commands

public interface ICommand<TResult>
{
}

public interface ICommandHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    Task<TResult> ExecuteAsync(
        TCommand command);
}

public sealed class CommandDispatcher
{
    private readonly Dictionary<Type, object>
        _handlers = [];

    public CommandDispatcher()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in assemblies)
        {
            RegisterAssembly(
                assembly);
        }

    }

    public CommandDispatcher(
        params Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
        {
            RegisterAssembly(
                assembly);
        }
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

                var handler =
                    Activator.CreateInstance(
                        type);

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

#endregion

#region Runtime

public sealed class DesignerRuntime
{
    public List<DesignDocument> Documents { get; }
        = [];

    public EventBus Events { get; }
        = new();

    public CommandDispatcher Commands { get; }
        = new();

    public DesignerRuntime()
    {
        //    Commands.Register(
        //        new CreateDocumentCommandHandler(this));

        //    Commands.Register(
        //        new CreateLayerCommandHandler(this));

        //    Commands.Register(
        //        new CreateRectangleCommandHandler(this));

        //    Commands.Register(
        //        new SelectCommandHandler<Layer>(this));

        //    Commands.Register(
        //        new SelectCommandHandler<SceneNode>(this));

        //    Commands.Register(
        //        new ClearSelectionCommandHandler<Layer>(this));

        //    Commands.Register(
        //        new ClearSelectionCommandHandler<SceneNode>(this));
        //}
    }

#endregion

    #region Results

    public sealed class SelectionResult<T>
    {
        public required ISelectionOwner<T> Owner
        {
            get;
            init;
        }

        public required IReadOnlyCollection<T> Items
        {
            get;
            init;
        }
    }

    #endregion

    #region Commands

    public sealed record CreateDocumentCommand(
        string Name)
        : ICommand<DesignDocument>;

    public sealed record CreateLayerCommand(
        DesignDocument Document,
        string Name)
        : ICommand<Layer>;

    public sealed record CreateRectangleCommand(
        Layer Layer,
        string Name)
        : ICommand<RectangleShape>;

    public sealed record SelectCommand<T>(
        ISelectionOwner<T> Owner,
        T Target)
        : ICommand<SelectionResult<T>>;

    public sealed record ClearSelectionCommand<T>(
        ISelectionOwner<T> Owner)
        : ICommand<SelectionResult<T>>;

    #endregion

    #region Handlers

    public sealed class CreateDocumentCommandHandler
        : ICommandHandler<
            CreateDocumentCommand,
            DesignDocument>
    {
        private readonly DesignerRuntime _runtime;

        public CreateDocumentCommandHandler(
            DesignerRuntime runtime)
        {
            _runtime = runtime;
        }

        public async Task<DesignDocument>
            ExecuteAsync(
                CreateDocumentCommand command)
        {
            var document =
                new DesignDocument
                {
                    Name = command.Name
                };

            _runtime.Documents.Add(
                document);

            await _runtime.Events.PublishAsync(
                new DocumentCreatedEvent(
                    document));

            return document;
        }
    }

    public sealed class CreateLayerCommandHandler
        : ICommandHandler<
            CreateLayerCommand,
            Layer>
    {
        private readonly DesignerRuntime _runtime;

        public CreateLayerCommandHandler(
            DesignerRuntime runtime)
        {
            _runtime = runtime;
        }

        public async Task<Layer>
            ExecuteAsync(
                CreateLayerCommand command)
        {
            var layer =
                new Layer
                {
                    Name = command.Name
                };

            command.Document.Layers.Add(
                layer);

            await _runtime.Events.PublishAsync(
                new LayerCreatedEvent(
                    command.Document,
                    layer));

            return layer;
        }
    }

    public sealed class CreateRectangleCommandHandler
        : ICommandHandler<
            CreateRectangleCommand,
            RectangleShape>
    {
        private readonly DesignerRuntime _runtime;

        public CreateRectangleCommandHandler(
            DesignerRuntime runtime)
        {
            _runtime = runtime;
        }

        public async Task<RectangleShape>
            ExecuteAsync(
                CreateRectangleCommand command)
        {
            var rectangle =
                new RectangleShape
                {
                    Name = command.Name
                };

            command.Layer.Children.Add(
                rectangle);

            await _runtime.Events.PublishAsync(
                new RectangleCreatedEvent(
                    command.Layer,
                    rectangle));

            return rectangle;
        }
    }

    public sealed class SelectCommandHandler<T>
        : ICommandHandler<
            SelectCommand<T>,
            SelectionResult<T>>
    {
        private readonly DesignerRuntime _runtime;

        public SelectCommandHandler(
            DesignerRuntime runtime)
        {
            _runtime = runtime;
        }

        public async Task<SelectionResult<T>>
            ExecuteAsync(
                SelectCommand<T> command)
        {
            command.Owner.Selection.Select(
                command.Target);

            var result =
                new SelectionResult<T>
                {
                    Owner = command.Owner,
                    Items =
                        command.Owner.Selection.Items
                            .ToList()
                };

            await _runtime.Events.PublishAsync(
                new SelectionChangedEvent<T>(
                    command.Owner,
                    result.Items));

            return result;
        }
    }

    public sealed class ClearSelectionCommandHandler<T>
        : ICommandHandler<
            ClearSelectionCommand<T>,
            SelectionResult<T>>
    {
        private readonly DesignerRuntime _runtime;

        public ClearSelectionCommandHandler(
            DesignerRuntime runtime)
        {
            _runtime = runtime;
        }

        public async Task<SelectionResult<T>>
            ExecuteAsync(
                ClearSelectionCommand<T> command)
        {
            command.Owner.Selection.Clear();

            var result =
                new SelectionResult<T>
                {
                    Owner = command.Owner,
                    Items = []
                };

            await _runtime.Events.PublishAsync(
                new SelectionChangedEvent<T>(
                    command.Owner,
                    result.Items));

            return result;
        }
    }

    #endregion

    #region Demo

    public static class Program
    {
        public static async Task Main()
        {
            var runtime =
                new DesignerRuntime();

            runtime.Events.Subscribe<
                LayerCreatedEvent>(
                evt =>
                {
                    Console.WriteLine(
                        $"Layer Created => {evt.Layer.Name}");

                    return Task.CompletedTask;
                });

            var document =
                await runtime.Commands.ExecuteAsync(
                    new CreateDocumentCommand(
                        "Landing Page"));

            var layer =
                await runtime.Commands.ExecuteAsync(
                    new CreateLayerCommand(
                        document,
                        "Desktop"));

            var hero =
                await runtime.Commands.ExecuteAsync(
                    new CreateRectangleCommand(
                        layer,
                        "Hero Banner"));

            var selectedLayers =
                await runtime.Commands.ExecuteAsync(
                    new SelectCommand<Layer>(
                        document,
                        layer));

            var selectedObjects =
                await runtime.Commands.ExecuteAsync(
                    new SelectCommand<SceneNode>(
                        layer,
                        hero));

            Console.WriteLine(
                $"Selected Layers : {selectedLayers.Items.Count}");

            Console.WriteLine(
                $"Selected Objects : {selectedObjects.Items.Count}");
        }
    }
    #endregion
}
