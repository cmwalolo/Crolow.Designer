namespace Crolow.Designer.Core.Animation;

public sealed class Timeline
{
    public double Duration { get; set; }

    public List<Track> Tracks { get; set; } = [];
}
