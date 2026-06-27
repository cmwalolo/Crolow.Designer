namespace Crolow.Designer.Core.Geometry;

public struct Rect2D
{
    public Rect2D(float x, float y, float width, float height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    public float X { get; set; }
    public float Y { get; set; }
    public float Width { get; set; }
    public float Height { get; set; }

    public float Right { get { return X + Width; } }
    public float Bottom { get { return Y + Height; } }
}
