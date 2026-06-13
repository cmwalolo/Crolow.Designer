using Crolow.Designer.Core.Styling.Effects;
using Crolow.Designer.Core.Styling.Strokes;

namespace Crolow.Designer.Core.Styling;

public class Appearance
{
    public List<FillStyle> Fills { get; set; } = [];

    public List<StrokeStyle> Strokes { get; set; } = [];

    public List<Effect> Effects { get; set; } = [];
}
