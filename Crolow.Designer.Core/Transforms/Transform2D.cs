using Crolow.Designer.Core.Geometry;

namespace Crolow.Designer.Core.Transforms;

public sealed class Transform2D
{
    public Point2D Position { get; set; }

    public double Rotation { get; set; }

    public double SkewX { get; set; }

    public double SkewY { get; set; }
}
