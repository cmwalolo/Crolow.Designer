using Crolow.Designer.Core.Geometry;

namespace Crolow.Designer.Wpf.App.Views.Document.Canvas.SelectionEditors;

public sealed class SelectionTransformChangedEventArgs : EventArgs
{
    public SelectionTransformChangedEventArgs()
    {
    }

    public SelectionTransformChangedEventArgs(Rect2D initSelection, Rect2D selection, float rotation)
    {
        InitSelection = initSelection;
        Selection = selection;
        Rotation = rotation;
    }

    public Rect2D InitSelection { get; set; }
    public Rect2D Selection { get; set; }
    public float Rotation { get; set; }

    public float X => Selection.X;
    public float Y => Selection.Y;
    public float Width => Selection.Width;
    public float Height => Selection.Height;
}