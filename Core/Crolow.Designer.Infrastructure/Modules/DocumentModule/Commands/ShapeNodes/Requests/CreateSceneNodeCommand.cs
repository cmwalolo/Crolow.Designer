using Crolow.Designer.Core.Scene.Nodes;
using Crolow.Designer.Runtime.Application.Commands;

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
