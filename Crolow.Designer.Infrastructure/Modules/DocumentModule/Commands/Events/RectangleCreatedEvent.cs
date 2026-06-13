using Crolow.Designer.Core.Scene.Nodes;
using Crolow.Designer.Core.Scene.Nodes.Objects;

namespace Crolow.Designer.Runtime.Modules.DocumentModule.Commands.Events;

public sealed record RectangleCreatedEvent(
    GroupNode Layer,
    RectangleShape Rectangle)
    : IEvent;
