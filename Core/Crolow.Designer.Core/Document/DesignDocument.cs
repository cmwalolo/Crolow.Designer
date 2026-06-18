using Crolow.Designer.Core.Geometry;
using Crolow.Designer.Core.Scene.Nodes;

namespace Crolow.Designer.Core.Document;

public class DesignDocument : IDataObject
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = new();
    public Size2D Size { get; set; }
    public List<LayerNode> Layers { get; set; } = [];
    public string FilePath { get; set; } = "";
}
