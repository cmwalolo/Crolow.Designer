using Crolow.Designer.Common.Runtime;
using Crolow.Designer.Core.Document;
using Crolow.Designer.Core.Scene.Nodes;

namespace Crolow.Designer.Runtime.Modules.DocumentModule.Commands.Layers.Requests;

public sealed record CreateLayerCommand : ICommandParameter<DesignDocument, LayerNode, LayerNode>
{
    public CreateLayerCommand(DesignDocument document, LayerNode layer)
    {
        Initiator = document;
        Request = layer;
    }

    public DesignDocument Initiator { get; set; }
    public LayerNode Request { get; set; }
}
