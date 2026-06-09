namespace Crolow.Designer.Core.Colors;

public class ColorPalette
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public List<ColorDefinition> Colors { get; set; } = new();
}
public class ColorDefinition
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public Color32 Color { get; set; }
}

public struct Color32
{
    public byte A { get; set; }

    public byte R { get; set; }

    public byte G { get; set; }

    public byte B { get; set; }
}
