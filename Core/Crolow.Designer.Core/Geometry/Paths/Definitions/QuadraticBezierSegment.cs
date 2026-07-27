namespace Crolow.Designer.Core.Geometry.Paths.Definitions;

public class QuadraticBezierSegment : PathSegment
{
    public QuadraticBezierSegment(Point2D point) : base(point)
    {
    }

    public Point2D ControlPoint { get; set; }
}
