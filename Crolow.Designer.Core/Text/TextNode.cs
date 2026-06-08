using Crolow.Designer.Core.Scene;
using Crolow.Designer.Core.Styling;

namespace Crolow.Designer.Core.Text;

public class TextNode : SceneNode
{
    public string Value { get; set; } = "";

    public string FontFamily { get; set; } = "";

    public double FontSize { get; set; }

    public Appearance Appearance { get; set; } = new();
}
