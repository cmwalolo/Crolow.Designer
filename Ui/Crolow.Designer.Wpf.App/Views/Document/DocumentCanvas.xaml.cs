using Crolow.Designer.Core.Geometry;
using Crolow.Designer.UI;
using Crolow.Designer.UI.Utils;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Crolow.Designer.Wpf.App.Views.Document
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class DocumentCanvas : UserControl, IDisposable
    {

        // Holds information of the current Canvas layout and config
        private CurrentCanvasSettings canvasSettings = new CurrentCanvasSettings();
        // Holds information about the current documentSettings
        private DesignDocumentSettings documentSettings = new DesignDocumentSettings();
        private DocumentController documentController;

        public DocumentCanvas(DocumentController documentController)
        {
            InitializeComponent();
            this.documentController = documentController;
            var dpi = VisualTreeHelper.GetDpi(this);
            canvasSettings.ScaleX = (float)dpi.DpiScaleX;
            canvasSettings.ScaleY = (float)dpi.DpiScaleY;
            documentSettings.Document = documentController.Session.Document;
            documentSettings.CurrentPage = documentController.ActivePage;
            Loaded += DocumentCanvas_Loaded;
        }

        private void DocumentCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            SetCanvasToFitOrFill();
        }

        #region Canvas & zooming 
        public void SetCanvasToFitOrFill()
        {
            var doc = documentSettings.CurrentPage;
            SKRect docRect = canvasSettings.ScaleToDpi(new Rect2D(0, 0, doc.Size.Width, doc.Size.Height));

            // -100 is to leave a margin in the SkOverlay to draw the document area
            float scaleX = ((float)SkOverlay.ActualWidth - 100) / docRect.Width;
            float scaleY = ((float)SkOverlay.ActualHeight - 100) / docRect.Height;

            canvasSettings.ZoomFactor = 1;

            if (scaleX < 1 || scaleY < 1)
            {
                canvasSettings.ZoomFactor = Math.Min(scaleX, scaleY);
                docRect = canvasSettings.ScaleToDpi(new Rect2D(0, 0, doc.Size.Width, doc.Size.Height));
            }
            float x = ((float)SkOverlay.ActualWidth - docRect.Width) / 2;
            float y = ((float)SkOverlay.ActualHeight - docRect.Height) / 2;
            docRect.Left += x;
            docRect.Right += x;
            docRect.Top += y;
            docRect.Bottom += y;
            canvasSettings.CanvasArea = docRect;
            SkCanvas.InvalidateVisual();
        }
        #endregion

        #region Overlay 
        private void OnOverlayMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                canvasSettings.IsSelected = false;
                canvasSettings.IsDragging = false;
                canvasSettings.IsDrawing = true;

                var p = e.GetPosition(SkOverlay);
                canvasSettings.CurrentPoint = new SKPoint((float)p.X, (float)p.Y);
                canvasSettings.CurentSelectionArea.Left = (float)p.X;
                canvasSettings.CurentSelectionArea.Top = (float)p.Y;
                canvasSettings.CurentSelectionArea.Right = (float)p.X;
                canvasSettings.CurentSelectionArea.Bottom = (float)p.Y;
                // Capture the mouse to track movement even if it leaves the element bounds
                SkOverlay.CaptureMouse();
            }
        }

        private void OnOverlayMouseMove(object sender, MouseEventArgs e)
        {
            if (canvasSettings.IsDrawing)
            {
                var p = e.GetPosition(SkOverlay);
                canvasSettings.CurrentPoint = new SKPoint((float)p.X, (float)p.Y);
                SkOverlay.InvalidateVisual();
            }
        }

        private void OnOverlayMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (canvasSettings.IsDrawing && e.ChangedButton == MouseButton.Left)
            {
                canvasSettings.IsDrawing = false;
                canvasSettings.IsSelected = true;
                documentSettings.IsSelected = true;

                SkOverlay.ReleaseMouseCapture();

                float left = (float)Math.Min(canvasSettings.CurentSelectionArea.Left, canvasSettings.CurrentPoint.X);
                float top = (float)Math.Min(canvasSettings.CurentSelectionArea.Top, canvasSettings.CurrentPoint.Y);
                float right = (float)Math.Max(canvasSettings.CurentSelectionArea.Right, canvasSettings.CurrentPoint.X);
                float bottom = (float)Math.Max(canvasSettings.CurentSelectionArea.Bottom, canvasSettings.CurrentPoint.Y);
                canvasSettings.CurentSelectionArea =
                    new SKRect(left, top, right, bottom);

                if (!canvasSettings.IsDragging &&
                        (canvasSettings.CurentSelectionArea.Width <= 4 || canvasSettings.CurentSelectionArea.Height <= 4))
                {
                    canvasSettings.IsRectangleSelected = false;
                    SkOverlay.InvalidateVisual();
                    return;
                }
                canvasSettings.IsRectangleSelected = true;
                documentSettings.CurrentSelectionArea = canvasSettings.ScaleToPixels(canvasSettings.CurentSelectionArea);

                SkOverlay.InvalidateVisual();

            }
        }

        private void OnOverlayPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKCanvas canvas = e.Surface.Canvas;
            canvas.Clear(SKColors.Transparent); // Base background color

            // Only render a rectangle if the user has clicked and dragged
            if (canvasSettings.IsDrawing || canvasSettings.IsRectangleSelected)
            {
                // Define the cosmetic style of your rectangle
                using (var paint = new SKPaint())
                {
                    paint.Style = SKPaintStyle.Stroke;
                    paint.Color = SKColors.Blue;
                    paint.StrokeWidth = 3;

                    SKRect rect = canvasSettings.CurentSelectionArea;
                    if (canvasSettings.IsDrawing)
                    {
                        // Calculate the standard coordinates (handles dragging in any direction)
                        float left = (float)Math.Min(canvasSettings.CurentSelectionArea.Left, canvasSettings.CurrentPoint.X);
                        float top = (float)Math.Min(canvasSettings.CurentSelectionArea.Top, canvasSettings.CurrentPoint.Y);
                        float right = (float)Math.Max(canvasSettings.CurentSelectionArea.Right, canvasSettings.CurrentPoint.X);
                        float bottom = (float)Math.Max(canvasSettings.CurentSelectionArea.Bottom, canvasSettings.CurrentPoint.Y);
                        rect = new SKRect(left, top, right, bottom);

                        if (!canvasSettings.IsDragging &&
                                (rect.Width <= 4 || rect.Height <= 4))
                        {
                            canvasSettings.IsDragging = false;
                            return;
                        }

                        canvasSettings.IsDragging = true;

                        // Create the drawing boundary
                        float[] intervals = new float[] { 10, 10 };
                        paint.PathEffect = SKPathEffect.CreateDash(intervals, 0);
                    }

                    // Draw the rectangle to the active canvas surface
                    canvas.DrawRect(rect, paint);
                    paint.PathEffect?.Dispose();
                }
            }
        }
        #endregion

        #region Canvas Surface
        private void OnCanvasPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKCanvas canvas = e.Surface.Canvas;
            canvas.Clear(SKColors.Transparent); // Base background color

            using (var paint = new SKPaint())
            {
                paint.IsAntialias = true;
                paint.Style = SKPaintStyle.Fill;
                paint.Color = SKColors.White;
                canvas.DrawRect(canvasSettings.CanvasArea, paint);
            }
        }

        #endregion 
        public void Dispose()
        {
        }

        private void ToggleButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }
        private void EditDocument_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
