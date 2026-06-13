
using Crolow.Designer.Core.Document;

namespace Crolow.Designer.Core.Colors;

public class ColorPalette : IDataObject
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public List<ColorDefinition> Colors { get; set; } = new();
}
