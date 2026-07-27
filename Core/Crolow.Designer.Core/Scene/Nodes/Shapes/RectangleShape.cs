using Crolow.Designer.Core.Geometry.Radius;

namespace Crolow.Designer.Core.Scene.Nodes.Objects;

public class RectangleShape : SceneNode
{
    public CornerRadiusValue[] CornerRadiusValues { get; set; } = [];

    public void ToggleDefaultCornerRadiusValue()
    {
        UseDefaultCornerRadiusValue = !UseDefaultCornerRadiusValue;

        if (UseDefaultCornerRadiusValue)
        {
            CornerRadiusValues = new CornerRadiusValue[4];
            for (int x = 0; x < 4; x++)
            {
                CornerRadiusValues[x] = DefaultCornerRadiusValue;
            }
        }
    }
}
