using Crolow.Designer.Core.Geometry;
using SkiaSharp;

namespace Crolow.Designer.UI.Utils
{
    public static class CanvasSettingsExtensions
    {
        extension(CurrentCanvasSettings settings)
        {
            public float ScaleToPixels(float p, bool horizontal)
            {
                return p / (horizontal ? settings.ScaleX : settings.ScaleY)
                          * settings.ZoomFactor;
            }
            public float ScaleToDpi(float p, bool horizontal)
            {
                return p * (horizontal ? settings.ScaleX : settings.ScaleY) / settings.ZoomFactor;
            }

            public Point2D ScaleToPixels(SKPoint p)
            {
                return new Point2D(p.X / settings.ScaleX * settings.ZoomFactor,
                                 p.Y / settings.ScaleY * settings.ZoomFactor);
            }
            public Point2D ScaleToDpi(SKPoint p)
            {
                return new Point2D(p.X * settings.ScaleX / settings.ZoomFactor,
                                 p.Y * settings.ScaleY / settings.ZoomFactor);
            }

            public Rect2D ScaleToPixels(SKRect p, float offsetX = 0, float offsetY = 0)
            {
                float left = settings.ScaleToPixels(p.Left - offsetX, true);
                float top = settings.ScaleToPixels(p.Top - offsetY, false);
                float right = settings.ScaleToPixels(p.Width, true);
                float bottom = settings.ScaleToPixels(p.Height, false);
                return new Rect2D(left, top, right, bottom);
            }

            public SKRect ScaleToDpi(Rect2D p, float offsetX = 0, float offsetY = 0)
            {
                float left = settings.ScaleToPixels(p.X + offsetX, true);
                float top = settings.ScaleToPixels(p.Y + offsetY, false);
                float width = settings.ScaleToPixels(p.Width, true);
                float height = settings.ScaleToPixels(p.Height, false);
                return new SKRect(left, top, left + width, left + height);
            }
        }
    }
}
