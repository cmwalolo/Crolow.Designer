namespace Crolow.Designer.Common.Data;

public class DataTreeObject : DataObject, IDataTreeObject<DataObject>
{
    public List<DataObject> Children { get; set; } = new();
}
