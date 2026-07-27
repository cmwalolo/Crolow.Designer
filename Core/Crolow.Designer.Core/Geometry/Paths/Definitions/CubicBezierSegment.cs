namespace Crolow.Designer.Core.Geometry.Paths.Definitions;

public class CubicBezierSegment : PathSegment
{
    public CubicBezierSegment(Point2D control1, Point2D control2, Point2D point) : base(point)
    {
        Control1 = control1;
        Control2 = control2;
    }

    public Point2D Control1 { get; set; }

    public Point2D Control2 { get; set; }
}
