using Crolow.Designer.Core.Geometry;
using Crolow.Designer.Core.Styling;
using Crolow.Designer.Core.Transforms;

namespace Crolow.Designer.Core.Scene.Nodes;

public abstract class SceneNode
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = "";

    public string? Tag { get; set; }

    public bool Visible { get; set; } = true;

    public double Opacity { get; set; } = 1.0;

    public Rect2D Canvas { get; set; }

    public Transform2D Transform { get; set; } = new();

    public Guid? ParentId { get; set; }

    public Appearance Appearance { get; set; } = new();

}
