using Crolow.Designer.Common.Constants;

namespace Crolow.Designer.Common.Event;

public class EventContainer<T>
{
    public EventAction EventAction { get; set; }
    public T Source { get; set; }
}
