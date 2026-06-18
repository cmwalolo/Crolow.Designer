using Crolow.Designer.Core.Styling.Brushes;

namespace Crolow.Designer.Core.Styling;

public class FillStyle
{
    public bool Enabled { get; set; } = true;

    public Brush Brush { get; set; } = default!;
}
