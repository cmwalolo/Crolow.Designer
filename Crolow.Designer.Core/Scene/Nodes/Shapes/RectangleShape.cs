using Crolow.Designer.Core.Styling.Strokes.Definitions;

namespace Crolow.Designer.Core.Scene.Nodes.Objects;

public sealed class RectangleShape : SceneNode
{
    public double Width { get; set; }

    public double Height { get; set; }

    public CornerRadius CornerRadius { get; set; } = new();
}
