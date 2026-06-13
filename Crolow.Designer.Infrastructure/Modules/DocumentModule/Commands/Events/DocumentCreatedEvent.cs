namespace Crolow.Designer.Runtime.Modules.DocumentModule.Commands.Events;

public sealed record DocumentCreatedEvent(
    DocumentSession DocumentSession)
    : IEvent;
