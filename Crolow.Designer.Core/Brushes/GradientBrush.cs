namespace Crolow.Designer.Core.Brushes;

public sealed class GradientBrush : Brush
{
    public GradientType Type { get; set; }

    public List<GradientStop> Stops { get; set; } = [];

    public Point2D StartPoint { get; set; }

    public Point2D EndPoint { get; set; }
}
