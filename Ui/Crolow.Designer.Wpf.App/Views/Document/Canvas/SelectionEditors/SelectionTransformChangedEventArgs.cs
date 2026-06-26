using Crolow.Designer.Core.Geometry;

namespace Crolow.Designer.Wpf.App.Views.Document.Canvas.SelectionEditors;

public sealed class SelectionTransformChangedEventArgs : EventArgs
{
    public SelectionTransformChangedEventArgs(Rect2D selection, float rotation)
    {
        Selection = selection;
        Rotation = rotation;
    }

    public Rect2D Selection { get; }
    public float Rotation { get; }

    public float X => Selection.X;
    public float Y => Selection.Y;
    public float Width => Selection.Width;
    public float Height => Selection.Height;
}