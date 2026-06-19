using Crolow.Designer.Common.Data;

namespace Crolow.Designer.Core.Scene.Nodes;

public class GroupNode : SceneNode, IDataTreeObject<SceneNode>
{
    public List<SceneNode> Children { get; set; } = [];
}
