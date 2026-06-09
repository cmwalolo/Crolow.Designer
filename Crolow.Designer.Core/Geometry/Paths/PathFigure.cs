using Crolow.Designer.Core.Geometry.Paths.Definitions;

namespace Crolow.Designer.Core.Geometry.Paths;

public sealed class PathFigure
{
    public Point2D StartPoint { get; set; }

    public bool Closed { get; set; }

    public List<PathSegment> Segments { get; set; } = [];
}
