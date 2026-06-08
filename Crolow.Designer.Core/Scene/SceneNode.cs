namespace Crolow.Designer.Core.Scene;

public abstract class SceneNode
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = "";

    public string? Tag { get; set; }

    public bool Visible { get; set; } = true;

    public double Opacity { get; set; } = 1.0;

    public Transform2D Transform { get; set; } = new();

    public Guid? ParentId { get; set; }
}
