using Crolow.Designer.Core.Scene.Nodes;
using Crolow.Designer.Core.Scene.Nodes.Objects;
using Crolow.Designer.Runtime.Application.Commands;

namespace Crolow.Designer.Runtime.Modules.DocumentModule.Commands.Requests;

public sealed record CreateRectangleCommand : ICommandParameter<GroupNode, object, RectangleShape>
{
    public CreateRectangleCommand(
        GroupNode parent)
    {
        Initiator = parent;
    }
    public GroupNode Initiator { get; set; }
    public object Request { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
}
