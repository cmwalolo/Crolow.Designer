using Crolow.Designer.Common.Runtime;
using Crolow.Designer.Core.Scene.Nodes;

namespace Crolow.Designer.Runtime.Modules.DocumentModule.Commands.Layers.Requests;

public sealed record CreateGroupNodeCommand : ICommandParameter<GroupNode, GroupNode, GroupNode>
{
    public CreateGroupNodeCommand(GroupNode parentNode, GroupNode requestNode)
    {
        Initiator = parentNode;
        Request = requestNode;

    }

    public GroupNode Initiator { get; set; }
    public GroupNode Request { get; set; }
}
