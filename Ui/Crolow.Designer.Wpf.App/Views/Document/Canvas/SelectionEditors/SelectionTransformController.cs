using Crolow.Designer.Core.Geometry;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Crolow.Designer.Wpf.App.Views.Document.Canvas.SelectionEditors;

public class SelectionTransformController : Control
{
    static SelectionTransformController()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
        typeof(SelectionTransformController),
        new FrameworkPropertyMetadata(typeof(SelectionTransformController)));
    }

    public SelectionTransformController()
    {
        Focusable = false;
        SnapsToDevicePixels = true;
    }

    #region Dependency Properties

    public static readonly DependencyProperty SelectionProperty =
        DependencyProperty.Register(
            nameof(Selection),
            typeof(Rect2D),
            typeof(SelectionTransformController),
            new FrameworkPropertyMetadata(default(Rect2D), FrameworkPropertyMetadataOptions.AffectsRender));

    public Rect2D Selection
    {
        get => (Rect2D)GetValue(SelectionProperty);
        set => SetValue(SelectionProperty, value);
    }

    public static readonly DependencyProperty RotationProperty =
        DependencyProperty.Register(
            nameof(Rotation),
            typeof(float),
            typeof(SelectionTransformController),
            new FrameworkPropertyMetadata(0f, FrameworkPropertyMetadataOptions.AffectsRender));

    /// <summary>
    /// Rotation in degrees around the center of Selection.
    /// </summary>
    public float Rotation
    {
        get => (float)GetValue(RotationProperty);
        set => SetValue(RotationProperty, value);
    }

    public static readonly DependencyProperty HandleSizeProperty =
        DependencyProperty.Register(
            nameof(HandleSize),
            typeof(float),
            typeof(SelectionTransformController),
            new FrameworkPropertyMetadata(10.0f, FrameworkPropertyMetadataOptions.AffectsRender));

    public float HandleSize
    {
        get => (float)GetValue(HandleSizeProperty);
        set => SetValue(HandleSizeProperty, value);
    }

    public static readonly DependencyProperty RotationHandleDistanceProperty =
        DependencyProperty.Register(
            nameof(RotationHandleDistance),
            typeof(float),
            typeof(SelectionTransformController),
            new FrameworkPropertyMetadata(24f, FrameworkPropertyMetadataOptions.AffectsRender));

    public float RotationHandleDistance
    {
        get => (float)GetValue(RotationHandleDistanceProperty);
        set => SetValue(RotationHandleDistanceProperty, value);
    }

    #endregion

    #region Events

    public event EventHandler<SelectionTransformChangedEventArgs>? IsChanging;
    public event EventHandler<SelectionTransformChangedEventArgs>? IsChanged;

    #endregion

    #region Public Geometry

    /// <summary>
    /// Returns the visual bounds of the controller including handles / rotation handle.
    /// Useful if parent wants to size overlay to fully contain the controller.
    /// </summary>
    public Rect2D OuterBounds
    {
        get
        {
            var r = GetSelectionRect();
            float half = HandleSize * 0.5f;
            float topExtra = RotationHandleDistance + HandleSize;

            return new Rect2D(
                (float)r.X - half,
                (float)r.Y - half - topExtra,
                (float)r.Width + HandleSize,
                (float)r.Height + HandleSize + topExtra);
        }
    }

    #endregion

    #region Interaction State

    private DragMode _dragMode = DragMode.None;
    private Point _dragStartCanvasPoint;
    private Rect _dragStartSelection;
    private float _dragStartRotation;
    private Point _selectionCenter;
    private Vector _rotationStartVector;
    private float _dragStartAspectRatio = 1f;
    private SelectionTransformChangedEventArgs changingEventArgs = new();

    #endregion

    #region Rendering

    protected override void OnRender(DrawingContext dc)
    {
        base.OnRender(dc);

        var selection = GetSelectionRect();
        if (selection.Width <= 0 || selection.Height <= 0)
            return;

        var center = GetSelectionCenter(selection);

        dc.PushTransform(new RotateTransform(Rotation, center.X, center.Y));

        // Surface transparente pour capter les clics / drag à l'intérieur
        dc.DrawRectangle(Brushes.Transparent, null, selection);

        DrawSelectionFrame(dc, selection);
        DrawHandles(dc, selection);

        dc.Pop();
    }

    private void DrawSelectionFrame(DrawingContext dc, Rect rect)
    {
        var pen = new Pen(Brushes.DodgerBlue, 1.0);
        dc.DrawRectangle(null, pen, rect);

        // line to rotation handle
        var topMiddle = new Point(rect.Left + rect.Width / 2, rect.Top);
        var rotationHandle = GetRotationHandlePoint(rect);
        dc.DrawLine(pen, topMiddle, rotationHandle);
    }

    private void DrawHandles(DrawingContext dc, Rect rect)
    {
        foreach (var handle in GetResizeHandleRects(rect))
        {
            dc.DrawRectangle(Brushes.White, new Pen(Brushes.DodgerBlue, 1), handle);
        }

        var rotationRect = GetRotationHandleRect(rect);
        dc.DrawEllipse(Brushes.White, new Pen(Brushes.DodgerBlue, 1),
            new Point(rotationRect.X + rotationRect.Width / 2, rotationRect.Y + rotationRect.Height / 2),
            rotationRect.Width / 2,
            rotationRect.Height / 2);
    }

    #endregion

    #region Mouse Interaction

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        base.OnMouseDown(e);

        changingEventArgs.InitSelection = Selection;

        if (e.ChangedButton != MouseButton.Left)
            return;

        Focus();
        CaptureMouse();

        var mouse = e.GetPosition(this);
        var unrotated = ToLocalUnrotated(mouse);

        var selection = GetSelectionRect();
        _dragMode = HitTestHandle(unrotated, selection);
        if (_dragMode == DragMode.None)
        {
            ReleaseMouseCapture();
            return;
        }

        _dragStartCanvasPoint = unrotated;
        _dragStartSelection = selection;
        _dragStartRotation = Rotation;
        _selectionCenter = GetSelectionCenter(selection);

        _dragStartAspectRatio = _dragStartSelection.Height > 0
            ? (float)(_dragStartSelection.Width / _dragStartSelection.Height)
            : 1f;

        if (_dragMode == DragMode.Rotate)
        {
            _rotationStartVector = unrotated - _selectionCenter;
        }

        e.Handled = true;
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);

        var mouse = e.GetPosition(this);
        var unrotated = ToLocalUnrotated(mouse);

        if (_dragMode == DragMode.None)
        {
            UpdateCursor(unrotated);
            return;
        }

        if (!IsMouseCaptured)
            return;

        switch (_dragMode)
        {
            case DragMode.Move:
                ApplyMove(unrotated);
                break;

            case DragMode.Rotate:
                ApplyRotation(unrotated);
                break;

            default:
                ApplyResize(unrotated);
                break;
        }

        e.Handled = true;
    }

    protected override void OnMouseUp(MouseButtonEventArgs e)
    {
        base.OnMouseUp(e);

        if (e.ChangedButton != MouseButton.Left)
            return;

        if (_dragMode != DragMode.None)
        {
            changingEventArgs.Selection = Selection;
            changingEventArgs.Rotation = Rotation;
            IsChanged?.Invoke(this, changingEventArgs);
        }

        _dragMode = DragMode.None;
        if (IsMouseCaptured)
            ReleaseMouseCapture();

        e.Handled = true;
    }

    protected override void OnMouseLeave(MouseEventArgs e)
    {
        base.OnMouseLeave(e);

        if (_dragMode == DragMode.None)
            Cursor = Cursors.Arrow;
    }

    #endregion

    #region Resize / Rotate Logic

    private void ApplyMove(Point currentPoint)
    {
        var delta = currentPoint - _dragStartCanvasPoint;

        Selection = new Rect2D(
            (float)(_dragStartSelection.X + delta.X),
            (float)(_dragStartSelection.Y + delta.Y),
            (float)_dragStartSelection.Width,
            (float)_dragStartSelection.Height);

        RaiseIsChanging();
    }

    private void ApplyRotation(Point currentPoint)
    {
        var currentVector = currentPoint - _selectionCenter;
        if (_rotationStartVector.Length < float.Epsilon || currentVector.Length < float.Epsilon)
            return;

        float startAngle = (float)Math.Atan2(_rotationStartVector.Y, _rotationStartVector.X);
        float currentAngle = (float)Math.Atan2(currentVector.Y, currentVector.X);
        float delta = (currentAngle - startAngle) * 180.0f / (float)Math.PI;

        Rotation = NormalizeAngle(_dragStartRotation + delta);
        RaiseIsChanging();
    }

    private void ApplyResize(Point currentPoint)
    {
        var start = _dragStartSelection;

        float left = (float)start.Left;
        float top = (float)start.Top;
        float right = (float)start.Right;
        float bottom = (float)start.Bottom;

        switch (_dragMode)
        {
            case DragMode.Left:
            case DragMode.TopLeft:
            case DragMode.BottomLeft:
                left = (float)currentPoint.X;
                break;
        }

        switch (_dragMode)
        {
            case DragMode.Right:
            case DragMode.TopRight:
            case DragMode.BottomRight:
                right = (float)currentPoint.X;
                break;
        }

        switch (_dragMode)
        {
            case DragMode.Top:
            case DragMode.TopLeft:
            case DragMode.TopRight:
                top = (float)currentPoint.Y;
                break;
        }

        switch (_dragMode)
        {
            case DragMode.Bottom:
            case DragMode.BottomLeft:
            case DragMode.BottomRight:
                bottom = (float)currentPoint.Y;
                break;
        }

        bool keepAspect = Keyboard.Modifiers.HasFlag(ModifierKeys.Shift);

        if (keepAspect && _dragStartAspectRatio > 0f)
        {
            bool leftHandle = _dragMode is DragMode.Left or DragMode.TopLeft or DragMode.BottomLeft;
            bool rightHandle = _dragMode is DragMode.Right or DragMode.TopRight or DragMode.BottomRight;
            bool topHandle = _dragMode is DragMode.Top or DragMode.TopLeft or DragMode.TopRight;
            bool bottomHandle = _dragMode is DragMode.Bottom or DragMode.BottomLeft or DragMode.BottomRight;

            bool horizontalOnly = _dragMode is DragMode.Left or DragMode.Right;
            bool verticalOnly = _dragMode is DragMode.Top or DragMode.Bottom;
            bool cornerHandle = !horizontalOnly && !verticalOnly && _dragMode != DragMode.Move && _dragMode != DragMode.Rotate;

            float centerX = ((float)start.Left + (float)start.Right) * 0.5f;
            float centerY = ((float)start.Top + (float)start.Bottom) * 0.5f;

            //float centerX = ((float)start.Left + (float)start.Right) * 0.5f;
            //float centerY = ((float)start.Top + (float)start.Bottom) * 0.5f;

            if (horizontalOnly)
            {
                // largeur pilotée par la distance au centre
                float targetHalfWidth = Math.Abs((float)currentPoint.X - centerX);
                float targetWidth = targetHalfWidth * 2f;
                float targetHeight = targetWidth / _dragStartAspectRatio;
                float targetHalfHeight = targetHeight * 0.5f;

                left = centerX - targetHalfWidth;
                right = centerX + targetHalfWidth;
                top = centerY - targetHalfHeight;
                bottom = centerY + targetHalfHeight;
            }
            else if (verticalOnly)
            {
                // hauteur pilotée par la distance au centre
                float targetHalfHeight = Math.Abs((float)currentPoint.Y - centerY);
                float targetHeight = targetHalfHeight * 2f;
                float targetWidth = targetHeight * _dragStartAspectRatio;
                float targetHalfWidth = targetWidth * 0.5f;

                left = centerX - targetHalfWidth;
                right = centerX + targetHalfWidth;
                top = centerY - targetHalfHeight;
                bottom = centerY + targetHalfHeight;
            }
            else if (cornerHandle)
            {
                float width = right - left;
                float height = bottom - top;

                float absWidth = Math.Abs(width);
                float absHeight = Math.Abs(height);

                // On choisit la dimension dominante du drag
                float widthFromHeight = absHeight * _dragStartAspectRatio;
                float heightFromWidth = absWidth / _dragStartAspectRatio;

                if (absWidth > widthFromHeight)
                {
                    absWidth = widthFromHeight;
                }
                else
                {
                    absHeight = heightFromWidth;
                }

                if (leftHandle)
                    left = right - absWidth;
                else if (rightHandle)
                    right = left + absWidth;

                if (topHandle)
                    top = bottom - absHeight;
                else if (bottomHandle)
                    bottom = top + absHeight;
            }
        }

        NormalizeEdgePair(ref left, ref right, (float)MinWidth);
        NormalizeEdgePair(ref top, ref bottom, (float)MinHeight);

        Selection = new Rect2D(left, top, right - left, bottom - top);
        RaiseIsChanging();
    }


    private static void NormalizeEdgePair(ref float a, ref float b, float minSize)
    {
        if (b >= a)
        {
            if (b - a < minSize)
                b = a + minSize;
        }
        else
        {
            var temp = a;
            a = b;
            b = temp;

            if (b - a < minSize)
                b = a + minSize;
        }
    }

    #endregion

    #region Hit Testing / Cursor

    private void UpdateCursor(Point point)
    {
        var selection = GetSelectionRect();
        var mode = HitTestHandle(point, selection);

        Cursor = mode switch
        {
            DragMode.TopLeft or DragMode.BottomRight => Cursors.SizeNWSE,
            DragMode.TopRight or DragMode.BottomLeft => Cursors.SizeNESW,
            DragMode.Left or DragMode.Right => Cursors.SizeWE,
            DragMode.Top or DragMode.Bottom => Cursors.SizeNS,
            DragMode.Rotate => Cursors.Hand,
            DragMode.Move => Cursors.SizeAll,
            _ => Cursors.Arrow
        };
    }

    private DragMode HitTestHandle(Point point, Rect selection)
    {
        var handles = GetResizeHandleRects(selection);

        if (handles[(int)HandleKind.TopLeft].Contains(point)) return DragMode.TopLeft;
        if (handles[(int)HandleKind.Top].Contains(point)) return DragMode.Top;
        if (handles[(int)HandleKind.TopRight].Contains(point)) return DragMode.TopRight;
        if (handles[(int)HandleKind.Right].Contains(point)) return DragMode.Right;
        if (handles[(int)HandleKind.BottomRight].Contains(point)) return DragMode.BottomRight;
        if (handles[(int)HandleKind.Bottom].Contains(point)) return DragMode.Bottom;
        if (handles[(int)HandleKind.BottomLeft].Contains(point)) return DragMode.BottomLeft;
        if (handles[(int)HandleKind.Left].Contains(point)) return DragMode.Left;

        if (GetRotationHandleRect(selection).Contains(point))
            return DragMode.Rotate;

        if (selection.Contains(point))
            return DragMode.Move;

        return DragMode.None;
    }

    #endregion

    #region Geometry Helpers

    private Rect GetSelectionRect()
        => new Rect(Selection.X, Selection.Y, Selection.Width, Selection.Height);

    private static Point GetSelectionCenter(Rect rect)
        => new(rect.Left + rect.Width / 2.0, rect.Top + rect.Height / 2.0);

    private Rect[] GetResizeHandleRects(Rect rect)
    {
        float hs = HandleSize;
        float hh = hs / 2.0f;

        var topLeft = new Rect(rect.Left - hh, rect.Top - hh, hs, hs);
        var top = new Rect(rect.Left + rect.Width / 2 - hh, rect.Top - hh, hs, hs);
        var topRight = new Rect(rect.Right - hh, rect.Top - hh, hs, hs);

        var right = new Rect(rect.Right - hh, rect.Top + rect.Height / 2 - hh, hs, hs);
        var bottomRight = new Rect(rect.Right - hh, rect.Bottom - hh, hs, hs);
        var bottom = new Rect(rect.Left + rect.Width / 2 - hh, rect.Bottom - hh, hs, hs);
        var bottomLeft = new Rect(rect.Left - hh, rect.Bottom - hh, hs, hs);
        var left = new Rect(rect.Left - hh, rect.Top + rect.Height / 2 - hh, hs, hs);

        return new[]
        {
        topLeft, top, topRight, right, bottomRight, bottom, bottomLeft, left
    };
    }

    private Rect GetRotationHandleRect(Rect rect)
    {
        float hs = HandleSize;
        float hh = hs / 2.0f;
        var p = GetRotationHandlePoint(rect);

        return new Rect(p.X - hh, p.Y - hh, hs, hs);
    }

    private Point GetRotationHandlePoint(Rect rect)
    {
        return new Point(rect.Left + rect.Width / 2.0, rect.Top - RotationHandleDistance);
    }

    private Point ToLocalUnrotated(Point p)
    {
        var selection = GetSelectionRect();
        var center = GetSelectionCenter(selection);

        if (Math.Abs(Rotation) < 0.0001f)
            return p;

        var matrix = new Matrix();
        matrix.RotateAt(-Rotation, center.X, center.Y);
        return matrix.Transform(p);
    }

    private void RaiseIsChanging()
    {
        changingEventArgs.Selection = Selection;
        changingEventArgs.Rotation = Rotation;

        IsChanging?.Invoke(this, changingEventArgs);
        InvalidateVisual();
    }

    private static float NormalizeAngle(float angle)
    {
        angle %= 360.0f;
        if (angle < 0)
            angle += 360.0f;
        return angle;
    }

    #endregion

    #region Internal Enums

    private enum HandleKind
    {
        TopLeft = 0,
        Top = 1,
        TopRight = 2,
        Right = 3,
        BottomRight = 4,
        Bottom = 5,
        BottomLeft = 6,
        Left = 7
    }

    private enum DragMode
    {
        None,
        Move,
        TopLeft,
        Top,
        TopRight,
        Right,
        BottomRight,
        Bottom,
        BottomLeft,
        Left,
        Rotate
    }

    #endregion
}
