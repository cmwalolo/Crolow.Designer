namespace Crolow.Designer.Runtime.Modules.DocumentModule.Commands.Events;

public sealed record SelectionChangedEvent<T>(
    IReadOnlyCollection<T> Items)
    : IEvent;
