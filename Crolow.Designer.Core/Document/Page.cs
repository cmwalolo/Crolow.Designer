namespace Crolow.Designer.Core.Document;

public sealed class Page
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = "";

    public double Width { get; set; }

    public double Height { get; set; }

    public List<Layer> Layers { get; set; } = [];
}
