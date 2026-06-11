using Crolow.Designer.Core.Document;

namespace Crolow.Designer.Runtime.Application.Events;

public sealed record DocumentCreatedEvent(
    DesignDocument Document)
    : IEvent;
