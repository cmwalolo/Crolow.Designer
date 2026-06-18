namespace Crolow.Designer.Core.Animation;

public sealed class Track
{
    public Guid TargetNodeId { get; set; }

    public string PropertyPath { get; set; } = "";

    public List<Keyframe> Keyframes { get; set; } = [];
}
