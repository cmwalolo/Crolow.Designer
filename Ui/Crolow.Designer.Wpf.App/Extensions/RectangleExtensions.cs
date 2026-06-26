using Crolow.Designer.Core.Geometry;
using SkiaSharp;

namespace Crolow.Designer.Wpf.App.Extensions
{
    public static class RectangleExtensions
    {
        public static SKRect ToSkRect(this Rect2D rect)
        {
            return new SKRect(rect.X, rect.Y, rect.X + rect.Width, rect.Y + rect.Height);
        }

        public static Rect2D ToRect2D(this SKRect rect)
        {
            return new Rect2D(rect.Left, rect.Top, rect.Width, rect.Height);
        }

    }
}
