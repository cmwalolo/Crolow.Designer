namespace Crolow.Designer.Core.Assets;

public sealed class ImageAsset
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = "";

    public string Source { get; set; } = "";
}
