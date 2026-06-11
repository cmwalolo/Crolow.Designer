using Crolow.Designer.Abstractions;

namespace Crolow.Designer.Runtime.Application.Events;

public sealed record SelectionChangedEvent<T>(
    ISelectionOwner<T> Owner,
    IReadOnlyCollection<T> Items)
    : IEvent;
