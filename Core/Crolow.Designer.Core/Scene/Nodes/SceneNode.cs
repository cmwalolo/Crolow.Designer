using Crolow.Designer.Common.Data;
using Crolow.Designer.Core.Geometry;
using Crolow.Designer.Core.Geometry.Paths;
using Crolow.Designer.Core.Styling;

namespace Crolow.Designer.Core.Scene.Nodes;

public abstract class SceneNode : DataObject
{
    public double Opacity { get; set; } = 1.0;

    public Rect2D Canvas { get; set; }
    public Rect2D FittingCanvas { get; set; }
    public float Rotation { get; set; } = new();

    public Appearance Appearance { get; set; } = new();
    public bool IsLocked { get; set; }
    public bool IsVisible { get; set; } = true;

    public PathGeometry BasicPath { get; set; }
    public PathGeometry TransformedPath { get; }
}
