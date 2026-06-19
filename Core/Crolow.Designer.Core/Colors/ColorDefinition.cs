
using Crolow.Designer.Common.Data;

namespace Crolow.Designer.Core.Colors;

public class ColorDefinition : DataObject
{
    public string Name { get; set; } = string.Empty;
    public Color32 Color { get; set; }
}
