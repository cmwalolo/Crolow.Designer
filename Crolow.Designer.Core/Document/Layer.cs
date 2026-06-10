using Crolow.Designer.Abstractions;
using Crolow.Designer.Core.Scene.Nodes;

namespace Crolow.Designer.Core.Document;

public sealed class Layer : ISelectionOwner<SceneNode>
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = "";

    public List<SceneNode> Children { get; set; } = [];
    public SelectionState<SceneNode> Selection { get; } = new SelectionState<SceneNode>();

}
