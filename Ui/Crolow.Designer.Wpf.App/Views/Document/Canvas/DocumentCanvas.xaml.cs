using Crolow.Designer.Core.Geometry;
using Crolow.Designer.Graphics.Core.Extensions;
using Crolow.Designer.Graphics.Core.UISettings;
using Crolow.Designer.Runtime.Modules.DocumentModule.Commands.Documents.Events;
using Crolow.Designer.UI;
using Crolow.Designer.UI.Enumerations;
using Crolow.Designer.Wpf.App.Extensions;
using Fluent;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Crolow.Designer.Wpf.App.Views.Document.Canvas
{
    public sealed class ToolboxButtonDefinition
    {
        public ToolboxTool Tool { get; init; }
        public string SvgSource { get; init; } = string.Empty;
    }

    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class DocumentCanvas : UserControl, IDisposable
    {
        private static readonly ToolboxButtonDefinition[] ToolboxDefinitions =
        [
            new() { Tool = ToolboxTool.SelectRectangle, SvgSource = "/Resources/Svg/Documents/ui-select-2.svg" },
            new() { Tool = ToolboxTool.Crop,            SvgSource = "/Resources/Svg/Documents/ui-crop.svg" },
            new() { Tool = ToolboxTool.Rectangle,       SvgSource = "/Resources/Svg/Documents/shape-rectangle.svg" },
            new() { Tool = ToolboxTool.Circle,          SvgSource = "/Resources/Svg/Documents/shape-circle.svg" },
            new() { Tool = ToolboxTool.Path,            SvgSource = "/Resources/Svg/Documents/shape-path.svg" },
            new() { Tool = ToolboxTool.Polygon,         SvgSource = "/Resources/Svg/Documents/shape-polygon.svg" },
            new() { Tool = ToolboxTool.Text,            SvgSource = "/Resources/Svg/Documents/shape-text.svg" },
            new() { Tool = ToolboxTool.DocumentRef,     SvgSource = "/Resources/Svg/Documents/ui-document-ref.svg" }
        ];

        private Fluent.ToggleButton[] toolboxButtons = Array.Empty<Fluent.ToggleButton>();

        // Holds information of the current Canvas layout and config
        public CurrentCanvasSettings canvasSettings { get; set; } = new CurrentCanvasSettings();
        // Holds information about the current documentSettings
        public DesignDocumentSettings documentSettings { get; set; } = new DesignDocumentSettings();
        private DocumentController documentController;
        private IDisposable documentSubscription;
        private IDisposable nodesSubscription;

        public DocumentCanvas(DocumentController documentController)
        {
            InitializeComponent();
            LoadToolbox();
            this.documentController = documentController;
            var dpi = VisualTreeHelper.GetDpi(this);
            canvasSettings.ScaleX = (float)dpi.DpiScaleX;
            canvasSettings.ScaleY = (float)dpi.DpiScaleY;
            documentSettings.Document = documentController.Session.Document;
            documentSettings.CurrentPage = documentController.ActivePage;
            Loaded += DocumentCanvas_Loaded;

            nodesSubscription = RuntimeController.Runtime.Events
                    .Subscribe<NodeEvent>(documentController.Session.Document.Id, OnSceneNodeEvent);
        }

        #region Controller Events

        private async Task OnSceneNodeEvent(NodeEvent args)
        {
            switch (args.EventAction)
            {
                case Common.Constants.EventAction.ObjectCreated:
                    CreateEditor();
                    break;
            }
        }
        #endregion 

        private void CreateEditor()
        {
            documentController.GetSelectionArea();
            SelectionController.Visibility = Visibility.Visible;
            SelectionController.CanvasSettings = canvasSettings;
            SelectionController.Selection = canvasSettings.CurentSelectionArea;
            SelectionController.Rotation = 0;
            SelectionController.InvalidateVisual();
            documentController.CurrentToolboxTool = ToolboxTool.None;
        }

        private void DocumentCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            SetCanvasToFitOrFill();
        }

        #region Canvas & zooming 
        public void SetCanvasToFitOrFill()
        {
            var doc = documentSettings.CurrentPage;
            Rect2D docRect = canvasSettings.ScaleToDpi(new Rect2D(0, 0, doc.Size.Width, doc.Size.Height));

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
            docRect.X += x;
            docRect.Y += y;
            canvasSettings.CanvasArea = docRect;
            SkCanvas.InvalidateVisual();
        }
        #endregion

        #region Overlay Mouse actions
        private void OnOverlayMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                canvasSettings.IsSelected = false;
                canvasSettings.IsDragging = false;
                canvasSettings.IsDrawing = true;

                var p = e.GetPosition(SkOverlay);
                canvasSettings.CurrentPoint = new Point2D((float)p.X, (float)p.Y);
                canvasSettings.CurentSelectionArea.X = (float)p.X;
                canvasSettings.CurentSelectionArea.Y = (float)p.Y;
                canvasSettings.CurentSelectionArea.Width = 1f;
                canvasSettings.CurentSelectionArea.Height = 1f;
                SkOverlay.CaptureMouse();
            }
        }

        private void OnOverlayMouseMove(object sender, MouseEventArgs e)
        {
            if (canvasSettings.IsDrawing)
            {
                var p = e.GetPosition(SkOverlay);
                canvasSettings.CurrentPoint = new Point2D((float)p.X, (float)p.Y);
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

                float left = (float)Math.Min(canvasSettings.CurentSelectionArea.X, canvasSettings.CurrentPoint.X);
                float top = (float)Math.Min(canvasSettings.CurentSelectionArea.Y, canvasSettings.CurrentPoint.Y);
                float right = (float)Math.Max(canvasSettings.CurentSelectionArea.Right, canvasSettings.CurrentPoint.X);
                float bottom = (float)Math.Max(canvasSettings.CurentSelectionArea.Bottom, canvasSettings.CurrentPoint.Y);
                canvasSettings.CurentSelectionArea = new Rect2D(left, top, right - left, bottom - top);

                if (!canvasSettings.IsDragging &&
                        (canvasSettings.CurentSelectionArea.Width <= 4 || canvasSettings.CurentSelectionArea.Height <= 4))
                {
                    canvasSettings.IsRectangleSelected = false;
                    SkOverlay.InvalidateVisual();
                    ProcessSelection();
                    return;
                }
                canvasSettings.IsRectangleSelected = true;
                documentSettings.CurrentSelectionArea = canvasSettings.ScaleToPixels(canvasSettings.CurentSelectionArea, canvasSettings.CanvasArea.X, canvasSettings.CanvasArea.Y);
                SkOverlay.InvalidateVisual();
                ProcessSelection();
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

                    Rect2D rect = canvasSettings.CurentSelectionArea;
                    if (canvasSettings.IsDrawing)
                    {
                        // Calculate the standard coordinates (handles dragging in any direction)
                        float left = (float)Math.Min(canvasSettings.CurentSelectionArea.X, canvasSettings.CurrentPoint.X);
                        float top = (float)Math.Min(canvasSettings.CurentSelectionArea.Y, canvasSettings.CurrentPoint.Y);
                        float right = (float)Math.Max(canvasSettings.CurentSelectionArea.Right, canvasSettings.CurrentPoint.X);
                        float bottom = (float)Math.Max(canvasSettings.CurentSelectionArea.Bottom, canvasSettings.CurrentPoint.Y);
                        rect = new Rect2D(left, top, right - left, bottom - top);

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

                        // Draw the rectangle to the active canvas surface
                        canvas.DrawRect(rect.ToSkRect(), paint);
                        paint.PathEffect?.Dispose();
                    }

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
                canvas.DrawRect(canvasSettings.CanvasArea.ToSkRect(), paint);
            }
        }

        #endregion 

        public void Dispose()
        {
            documentSubscription?.Dispose();
            nodesSubscription?.Dispose();
        }


        private void ToggleButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var tb = sender as ToggleButton;
            documentController.CurrentToolboxTool = (ToolboxTool)tb.Tag;
        }
        private void EditDocument_Click(object sender, RoutedEventArgs e)
        {

        }

        #region process Selection
        private void ProcessSelection()
        {
            switch (documentController.CurrentToolboxTool)
            {
                case ToolboxTool.Rectangle:
                    documentController.CreateRectangle(documentSettings);
                    break;
            }
            ClearToolbox();

        }

        #endregion 

        #region Toolbox
        private void ClearToolbox()
        {
            foreach (var button in toolboxButtons)
            {
                button.IsChecked = false;
            }
        }

        private void LoadToolbox()
        {
            toolboxContainer.Items.Clear();

            toolboxButtons = ToolboxDefinitions
                .Select(def =>
                {
                    var button = new Fluent.ToggleButton
                    {
                        Tag = def.Tool,
                        GroupName = "ToolboxTools",
                        Padding = new Thickness(0),
                        Margin = new Thickness(0),
                        Width = 34,
                        Height = 34
                    };

                    button.Click += ToggleButton_Click;

                    var svg = new SharpVectors.Converters.SvgViewbox
                    {
                        Source = new Uri(def.SvgSource, UriKind.Relative)
                    };

                    button.Icon = new Viewbox
                    {
                        Width = 12,
                        Height = 12,
                        Child = svg
                    };

                    toolboxContainer.Items.Add(button);
                    return button;
                }).ToArray();
        }
        #endregion

        private void SelectionController_IsChanged(object sender, Core.Transforms.TransformContent e)
        {
            float offsetX = canvasSettings.CanvasArea.X;
            float offsetY = canvasSettings.CanvasArea.Y;

            // documentSettings.CurrentSelectionArea = canvasSettings.ScaleToPixels(canvasSettings.CurentSelectionArea, canvasSettings.CanvasArea.X, canvasSettings.CanvasArea.Y);
        }

        private void SelectionController_IsChanging(object sender, Core.Transforms.TransformContent e)
        {

        }
    }
}
