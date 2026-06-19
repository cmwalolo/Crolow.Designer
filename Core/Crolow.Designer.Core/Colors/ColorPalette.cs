using Crolow.Designer.Common.Data;

namespace Crolow.Designer.Core.Colors;

public class ColorPalette : DataObject
{
    public string Name { get; set; } = string.Empty;
    public List<ColorDefinition> Colors { get; set; } = new();
}
