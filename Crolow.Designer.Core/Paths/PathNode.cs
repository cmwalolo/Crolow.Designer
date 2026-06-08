namespace Crolow.Designer.Core.Paths;

public sealed class PathNode : SceneNode
{
    public BezierPath Path { get; set; } = new();

    public Appearance Appearance { get; set; } = new();
}
