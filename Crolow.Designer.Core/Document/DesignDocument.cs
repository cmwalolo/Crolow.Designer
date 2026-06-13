using Crolow.Designer.Core.Scene.Nodes;

namespace Crolow.Designer.Core.Document;

public class DesignDocument : IDataObject
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = "";

    public double Width { get; set; }

    public double Height { get; set; }

    public List<LayerNode> Layers { get; set; } = [];

}
