using Crolow.Designer.Core.Geometry;
using Crolow.Designer.Graphics.Core.UISettings;

namespace Crolow.Designer.Graphics.Core.Extensions
{
    public static class CanvasSettingsExtensions
    {
        extension(CurrentCanvasSettings settings)
        {
            public float ScaleToPixels(float p, bool horizontal)
            {
                return p / (horizontal ? settings.ScaleX : settings.ScaleY)
                          / settings.ZoomFactor;
            }
            public float ScaleToDpi(float p, bool horizontal)
            {
                return p * (horizontal ? settings.ScaleX : settings.ScaleY) * settings.ZoomFactor;
            }

            public Point2D ScaleToPixels(Point2D p, float offsetX = 0, float offsetY = 0)
            {
                return new Point2D((p.X - offsetX) / settings.ScaleX / settings.ZoomFactor,
                                    (p.Y - offsetY) / settings.ScaleY / settings.ZoomFactor);
            }
            public Point2D ScaleToDpi(Point2D p, float offsetX = 0, float offsetY = 0)
            {
                return new Point2D((p.X + offsetX) * settings.ScaleX * settings.ZoomFactor,
                                 (p.Y + offsetY) * settings.ScaleY * settings.ZoomFactor);
            }

            public Rect2D ScaleToPixels(Rect2D p, float offsetX = 0, float offsetY = 0)
            {
                float left = settings.ScaleToPixels(p.X - offsetX, true);
                float top = settings.ScaleToPixels(p.Y - offsetY, false);
                float right = settings.ScaleToPixels(p.Width, true);
                float bottom = settings.ScaleToPixels(p.Height, false);
                return new Rect2D(left, top, right, bottom);
            }

            public Rect2D ScaleToDpi(Rect2D p, float offsetX = 0, float offsetY = 0)
            {
                float left = settings.ScaleToDpi(p.X + offsetX, true);
                float top = settings.ScaleToDpi(p.Y + offsetY, false);
                float width = settings.ScaleToDpi(p.Width, true);
                float height = settings.ScaleToDpi(p.Height, false);
                return new Rect2D(left, top, left + width, left + height);
            }
        }
    }
}
