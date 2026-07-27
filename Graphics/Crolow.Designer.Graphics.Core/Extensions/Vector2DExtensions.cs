using Crolow.Designer.Core.Geometry;

namespace Crolow.Designer.Graphics.Core.Extensions
{
    public static class Vector2DExtensions
    {
        extension(Vector2D left)
        {
            public Angle AngleTo(Vector2D right)
            {
                float dot = left.Dot(right);
                float det = left.Cross(right);

                return Angle.FromRadians(MathF.Atan2(det, dot));
            }

            public float Dot(Vector2D right)
            {
                return left.X * right.X +
                       left.Y * right.Y;
            }

            public float Cross(Vector2D right)
            {
                return left.X * right.Y -
                       left.Y * right.X;
            }
        }

        extension(Vector2D vector)
        {
            public float Length()
            {
                return MathF.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            }

            public float LengthSquared()
            {
                return vector.X * vector.X + vector.Y * vector.Y;
            }

            public Vector2D Normalize()
            {
                float length = vector.Length();

                if (length == 0)
                    return vector;

                return new Vector2D(
                    vector.X / length,
                    vector.Y / length);
            }

            public Vector2D Scale(float factor)
            {
                return new Vector2D(
                    vector.X * factor,
                    vector.Y * factor);
            }

            public Vector2D Rotate(Angle angle)
            {
                float cos = MathF.Cos(angle.Radians);
                float sin = MathF.Sin(angle.Radians);

                return new Vector2D(
                    vector.X * cos - vector.Y * sin,
                    vector.X * sin + vector.Y * cos);
            }

            public Vector2D PerpendicularLeft()
            {
                return new Vector2D(
                    -vector.Y,
                     vector.X);
            }

            public Vector2D PerpendicularRight()
            {
                return new Vector2D(
                     vector.Y,
                    -vector.X);
            }
        }
    }
}
