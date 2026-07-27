using Crolow.Designer.Core.Geometry;

namespace Crolow.Designer.Graphics.Core.Extensions
{
    public static class Rect2DExtensions
    {
        extension(Rect2D rect)
        {
            public bool IsEmpty()
                => rect.Width <= 0 || rect.Height <= 0;
            public Size2D Size()
                => new(rect.Width, rect.Height);
            public Point2D TopLeft()
                => new(rect.X, rect.Y);

            public Point2D TopRight()
                => new(rect.Right, rect.Y);

            public Point2D BottomRight()
                => new(rect.Right, rect.Bottom);

            public Point2D BottomLeft()
                => new(rect.X, rect.Bottom);

            public Point2D Center()
                => new(
                    rect.X + rect.Width * 0.5f,
                    rect.Y + rect.Height * 0.5f);

            public bool Contains(Point2D point)
                => point.X >= rect.X &&
                   point.X <= rect.Right &&
                   point.Y >= rect.Y &&
                   point.Y <= rect.Bottom;

            public Rect2D Inflate(float amount)
                => new(
                    rect.X - amount,
                    rect.Y - amount,
                    rect.Width + amount * 2,
                    rect.Height + amount * 2);

            public Rect2D Deflate(float amount)
                => rect.Inflate(-amount);

            public Rect2D Scale(float scaleX, float scaleY, bool keepPosition = true, bool keepCentered = false)
            {
                Point2D center = new Point2D(0, 0);

                if (keepCentered)
                {
                    center = rect.Center();
                }

                rect.Width *= scaleX;
                rect.Height *= scaleY;

                if (!keepPosition)
                {
                    rect.X *= scaleX;
                    rect.Y *= scaleY;
                }

                if (keepCentered)
                {
                    rect.X = center.X - (rect.Width / 2);
                    rect.Y = center.Y - (rect.Height / 2);
                }
                return rect;
            }
        }
    }
}
