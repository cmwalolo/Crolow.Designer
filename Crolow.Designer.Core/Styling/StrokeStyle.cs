namespace Crolow.Designer.Core.Styling;

public sealed class StrokeStyle
{
    public bool Enabled { get; set; } = true;

    public double Width { get; set; } = 1.0;

    public StrokeAlignment Alignment { get; set; }

    public LineJoin LineJoin { get; set; }

    public LineCap LineCap { get; set; }

    public Brush Brush { get; set; } = default!;

    public List<double>? DashPattern { get; set; }

    public double DashOffset { get; set; }
}
