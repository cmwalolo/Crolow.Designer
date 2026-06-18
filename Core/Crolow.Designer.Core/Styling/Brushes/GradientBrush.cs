using Crolow.Designer.Core.Geometry;
using Crolow.Designer.Core.Styling.Brushes.Definitions;

namespace Crolow.Designer.Core.Styling.Brushes;

public class GradientBrush : Brush
{
    public GradientType Type { get; set; }

    public List<GradientStop> Stops { get; set; } = [];

    public Point2D StartPoint { get; set; }

    public Point2D EndPoint { get; set; }
}
