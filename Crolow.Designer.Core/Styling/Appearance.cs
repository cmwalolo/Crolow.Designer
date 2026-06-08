using Crolow.Designer.Core.Effects;

namespace Crolow.Designer.Core.Styling;

public sealed class Appearance
{
    public List<FillStyle> Fills { get; set; } = [];

    public List<StrokeStyle> Strokes { get; set; } = [];

    public List<Effect> Effects { get; set; } = [];
}
