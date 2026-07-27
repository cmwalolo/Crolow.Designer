using Crolow.Designer.Core.Geometry;

namespace Crolow.Designer.Graphics.Core.Extensions
{
    public static class Point2DExtensions
    {
        extension(Point2D from)
        {
            public float DistanceTo(Point2D to)
            {
                float dx = to.X - from.X;
                float dy = to.Y - from.Y;

                return (float)MathF.Sqrt(dx * dx + dy * dy);
            }

            public Vector2D DirectionTo(Point2D to)
            {
                return new Vector2D(to.X - from.X, to.Y - from.Y);
            }

            public Point2D MoveTowards(Point2D to, float distance)
            {
                return from.Translate(
                    from.DirectionTo(to)
                        .Normalize()
                        .Scale(distance));
            }

            public Point2D MoveAwayFrom(Point2D to, float distance)
            {
                return from.Translate(
                    from.DirectionTo(to)
                        .Normalize()
                        .Scale(-distance));
            }
        }

        extension(Point2D p1)
        {
            public Point2D MidPoint(Point2D p2)
            {
                return new Point2D(
                    (p1.X + p2.X) * 0.5f,
                    (p1.Y + p2.Y) * 0.5f);
            }
        }

        extension(Point2D point)
        {
            public Point2D Translate(Vector2D vector)
            {
                return new Point2D(
                    point.X + vector.X,
                    point.Y + vector.Y);
            }
        }
    }
}
