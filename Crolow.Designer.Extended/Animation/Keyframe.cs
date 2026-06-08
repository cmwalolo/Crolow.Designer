namespace Crolow.Designer.Core.Animation;

public sealed class Keyframe
{
    public double Time { get; set; }

    public object? Value { get; set; }

    public EasingType Easing { get; set; }
}
