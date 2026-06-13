
using Crolow.Designer.Core.Document;

namespace Crolow.Designer.Core.Styling.Brushes;

public abstract class Brush : IDataObject
{
    public Guid Id { get; set; } = Guid.NewGuid();
}
