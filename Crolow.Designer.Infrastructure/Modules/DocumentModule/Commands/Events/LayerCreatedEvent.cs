using Crolow.Designer.Core.Scene.Nodes;

namespace Crolow.Designer.Runtime.Modules.DocumentModule.Commands.Events;

public sealed record LayerCreatedEvent(
    DocumentSession DocumentSession,
    LayerNode Layer)
    : IEvent;
