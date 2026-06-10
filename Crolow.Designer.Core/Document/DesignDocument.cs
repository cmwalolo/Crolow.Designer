using Crolow.Designer.Abstractions;

namespace Crolow.Designer.Core.Document;

public sealed class DesignDocument : ISelectionOwner<Layer>
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = "";

    public double Width { get; set; }

    public double Height { get; set; }

    public List<Layer> Layers { get; set; } = [];
    public SelectionState<Layer> Selection { get; } = new SelectionState<Layer>();

}
