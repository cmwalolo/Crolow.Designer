
using Crolow.Designer.Core.Document;

namespace Crolow.Designer.Core.Colors;

public class ColorDefinition : IDataObject
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public Color32 Color { get; set; }
}
