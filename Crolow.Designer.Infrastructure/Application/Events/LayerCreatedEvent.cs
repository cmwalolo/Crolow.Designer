using Crolow.Designer.Core.Document;

namespace Crolow.Designer.Runtime.Application.Events;

public sealed record LayerCreatedEvent(
    DesignDocument Document,
    Layer Layer)
    : IEvent;
