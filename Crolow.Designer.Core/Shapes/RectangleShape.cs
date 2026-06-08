namespace Crolow.Designer.Core.Shapes;

public sealed class RectangleShape : ShapeNode
{
    public double Width { get; set; }

    public double Height { get; set; }

    public CornerRadius CornerRadius { get; set; } = new();
}
