using Crolow.Designer.Core.Geometry;

namespace Crolow.Designer.Core.Transforms;

public class TransformContent : EventArgs
{
    public TransformContent()
    {
    }

    public TransformContent(Rect2D initSelection, Rect2D selection, float rotation)
    {
        InitSelection = initSelection;
        Selection = selection;
        Rotation = rotation;
    }

    public Rect2D InitSelection { get; set; }
    public Rect2D Selection { get; set; }
    public float Rotation { get; set; }
    public Point2D Offset { get; set; }
    public Point2D Scale { get; set; }
    public Point2D InitCenter { get; set; }
    public Point2D Center { get; set; }

    public float X => Selection.X;
    public float Y => Selection.Y;
    public float Width => Selection.Width;
    public float Height => Selection.Height;
}