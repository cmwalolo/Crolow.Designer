using System.Text.Json.Serialization;

namespace Crolow.Designer.Common.Data;

public class DataObject : IDataObject
{
    public EditState EditState { get; set; }
    public Guid ParentId { get; set; } = Guid.NewGuid();
    public Guid Id { get; set; } = Guid.NewGuid();
    public float Position { get; set; } = 0;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = new();

    [JsonIgnore]
    public IDataObject ParentNode { get; set; }
}
