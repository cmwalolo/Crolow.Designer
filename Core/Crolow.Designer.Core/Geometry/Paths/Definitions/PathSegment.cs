namespace Crolow.Designer.Core.Geometry.Paths.Definitions;

public abstract class PathSegment
{
    public PathSegment(Point2D point)
    {
        EndPoint = point;
    }
    public Point2D EndPoint { get; set; }
}
