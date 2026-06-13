using Crolow.Designer.Core.Document;
using Crolow.Designer.Core.Styling.Brushes;
using Crolow.Designer.Core.Styling.Strokes.Definitions;

namespace Crolow.Designer.Core.Styling.Strokes;

public class StrokeStyle : IDataObject
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public bool Enabled { get; set; } = true;

    public double Width { get; set; } = 1.0;

    public StrokeAlignment Alignment { get; set; }

    public LineJoin LineJoin { get; set; }

    public LineCap LineCap { get; set; }

    public Brush Brush { get; set; } = default!;

    public List<double>? DashPattern { get; set; }

    public double DashOffset { get; set; }
    public CornerRadius CornerRadius { get; set; }
}
