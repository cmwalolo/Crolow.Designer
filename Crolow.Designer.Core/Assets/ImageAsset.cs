using Crolow.Designer.Core.Document;

namespace Crolow.Designer.Core.Assets;

public class ImageAsset : IDataObject
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = "";

    public string Source { get; set; } = "";
}
