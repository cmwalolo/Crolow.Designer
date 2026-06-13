using Crolow.Designer.Core.Document;

namespace Crolow.Designer.Core.Styling.Effects;

public abstract class Effect : IDataObject
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public bool Enabled { get; set; } = true;
}
