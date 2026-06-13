using Crolow.Designer.Core.Document;
using Crolow.Designer.Core.Scene.Nodes;
using Crolow.Designer.Runtime.Application.Commands;

namespace Crolow.Designer.Runtime.Modules.DocumentModule.Commands.Requests;

public sealed record CreateLayerCommand : ICommandParameter<DesignDocument, object, LayerNode>
{
    public CreateLayerCommand(DesignDocument document)
    {
        Initiator = document;
    }

    public DesignDocument Initiator { get; set; }
    public object Request { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
}
