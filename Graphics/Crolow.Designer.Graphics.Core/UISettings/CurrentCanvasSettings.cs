using Crolow.Designer.Core.Geometry;

namespace Crolow.Designer.Graphics.Core.UISettings
{
    public class CurrentCanvasSettings
    {
        public bool IsDragging = false;
        public bool IsDrawing = false;
        public bool IsSelected = false;
        public bool IsRectangleSelected = false;
        public Point2D CurrentPoint;
        public Rect2D CurentSelectionArea;
        public float ZoomFactor = 1f;
        public Rect2D CanvasArea;
        public float ScaleX;
        public float ScaleY;
    }
}
