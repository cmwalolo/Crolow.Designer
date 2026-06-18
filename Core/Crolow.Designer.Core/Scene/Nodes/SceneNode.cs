using Crolow.Designer.Core.Document;
using Crolow.Designer.Core.Geometry;
using Crolow.Designer.Core.Styling;
using Crolow.Designer.Core.Transforms;
using System.Text.Json.Serialization;

namespace Crolow.Designer.Core.Scene.Nodes;

public abstract class SceneNode : IDataObject
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = "";

    public string? Tag { get; set; }

    public bool Visible { get; set; } = true;

    public double Opacity { get; set; } = 1.0;

    public Rect2D Canvas { get; set; }

    public Transform2D Transform { get; set; } = new();

    public Appearance Appearance { get; set; } = new();

    [JsonIgnore]
    public Guid? ParentId { get; set; }
    [JsonIgnore]
    public IDataObject? ParentNode { get; set; }

}
