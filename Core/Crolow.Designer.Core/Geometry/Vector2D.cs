namespace Crolow.Designer.Core.Geometry;

public readonly struct Vector2D
{
    public Vector2D(float x, float y)
    {
        X = x;
        Y = y;
    }
    public float X { get; }

    public float Y { get; }

    public float Length => (float)MathF.Sqrt(X * X + Y * Y);

    public float LengthSquared => X * X + Y * Y;
}