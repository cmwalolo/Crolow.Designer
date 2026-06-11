using Crolow.Designer.Core.Document;
using Crolow.Designer.Core.Scene.Nodes.Objects;

namespace Crolow.Designer.Runtime.Application.Events;

public sealed record RectangleCreatedEvent(
    Layer Layer,
    RectangleShape Rectangle)
    : IEvent;
