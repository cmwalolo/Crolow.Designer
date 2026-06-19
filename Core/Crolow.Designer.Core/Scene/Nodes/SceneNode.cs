using Crolow.Designer.Common.Data;
using Crolow.Designer.Core.Geometry;
using Crolow.Designer.Core.Styling;
using Crolow.Designer.Core.Transforms;
using System.Text.Json.Serialization;

namespace Crolow.Designer.Core.Scene.Nodes;

public abstract class SceneNode : DataObject
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = new();

    public bool Visible { get; set; } = true;

    public double Opacity { get; set; } = 1.0;

    public Rect2D Canvas { get; set; }

    public Transform2D Transform { get; set; } = new();

    public Appearance Appearance { get; set; } = new();
    public bool IsLocked { get; set; }
    public bool IsVisible { get; set; }

    [JsonIgnore]
    public IDataObject? ParentNode { get; set; }

}
