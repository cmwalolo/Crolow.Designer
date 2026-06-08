namespace Crolow.Designer.Core.Paths;

public sealed class PathFigure
{
    public Point2D StartPoint { get; set; }

    public bool Closed { get; set; }

    public List<PathSegment> Segments { get; set; } = [];
}
