using SkiaSharp;

namespace Crolow.Designer.UI.Utils
{
    public class CurrentCanvasSettings
    {
        public bool IsDrawing = false;
        public bool IsSelected = false;
        public SKPoint CurrentPoint;
        public SKRect CurentSelectionArea;
        public float ZoomFactor = 1f;
        public SKRect CanvasArea;
        public float ScaleX;
        public float ScaleY;
    }
}
