using Crolow.Designer.Common.Runtime;
using Crolow.Designer.Core.Scene.Nodes;

namespace Crolow.Designer.Runtime.Modules.DocumentModule.Commands.Shapes.Requests;

public sealed record CreateSceneNodeCommand : ICommandParameter<GroupNode, SceneNode, SceneNode>
{
    public CreateSceneNodeCommand(GroupNode parent, SceneNode request)
    {
        Initiator = parent;
        Request = request;
    }
    public GroupNode Initiator { get; set; }
    public SceneNode Request { get; set; }
}
