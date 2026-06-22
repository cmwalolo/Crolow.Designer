using Crolow.Designer.Common.Data;
using Crolow.Designer.Core.Geometry;
using Crolow.Designer.Core.Scene.Nodes;

namespace Crolow.Designer.Core.Document;

public class DesignDocument : DataObject
{
    public Size2D Size { get; set; }
    public List<PageNode> Pages { get; set; } = [];
    public string FilePath { get; set; } = "";
}
