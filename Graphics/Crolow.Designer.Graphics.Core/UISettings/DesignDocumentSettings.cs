using Crolow.Designer.Core.Document;
using Crolow.Designer.Core.Geometry;
using Crolow.Designer.Core.Scene.Nodes;

namespace Crolow.Designer.Graphics.Core.UISettings
{
    public class DesignDocumentSettings
    {
        public bool IsSelected = false;
        public DesignDocument Document;
        public PageNode CurrentPage;
        public Rect2D CurrentSelectionArea;
    }
}
