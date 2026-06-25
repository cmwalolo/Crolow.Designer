using Crolow.Designer.Core.Scene.Nodes;
using SkiaSharp;

namespace Crolow.Designer.UI.Interfaces
{
    public enum SelectionType
    {
        Selection,
        Creation
    }

    public class SelectionRequest
    {
        public SceneNode Node { get; set; }
        public SelectionType SelectionType { get; set; }
        public SKRect Selection { get; set; }
    }

    public interface ICanvasUI
    {
        public SelectionRequest RequestSelection();
        public SelectionRequest GetSelection();
    }
}
