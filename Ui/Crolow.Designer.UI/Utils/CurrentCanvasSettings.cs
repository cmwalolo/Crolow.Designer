using SkiaSharp;

namespace Crolow.Designer.UI.Utils
{
    public class CurrentCanvasSettings
    {
        public bool IsDragging = false;
        public bool IsDrawing = false;
        public bool IsSelected = false;
        public bool IsRectangleSelected = false;
        public SKPoint CurrentPoint;
        public SKRect CurentSelectionArea;
        public float ZoomFactor = 1f;
        public SKRect CanvasArea;
        public float ScaleX;
        public float ScaleY;
    }
}
