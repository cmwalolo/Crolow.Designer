using Crolow.Designer.Core.Scene.Nodes;

namespace Crolow.Designer.Core.Document;

public sealed class Layer
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = "";

    public List<SceneNode> Children { get; set; } = [];
}
