using Crolow.Designer.Core.Assets;

namespace Crolow.Designer.Core.Document;

public sealed class DesignDocument
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = "";

    public List<Page> Pages { get; set; } = [];

    public AssetLibrary Assets { get; set; } = new();
}
